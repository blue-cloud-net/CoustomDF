using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

/// <summary>
/// 
/// </summary>
public class RedisCache : IDistributedCache, ICacheSupportsMultipleItems, ICacheSupportsSortSet
{
    private const string HashSetScript = @"
                redis.call('HSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                    redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1";

    private const string SortedSetAddScript = @"
                redis.call('ZADD', KEYS[1], ARGV[2], ARGV[3])
                if ARGV[1] ~= '-1' then
                    redis.call('EXPIRE', KEYS[1], ARGV[1])
                end
                return 1";

    private volatile IConnectionMultiplexer? _connection;
    private IDatabase? _cache;

    private readonly IRedisConnectionMultiplexerFactory _connectionFactory;
    private readonly RedisCacheOptions _options;
    private readonly string _instanceName;
    private readonly ILogger _logger;

    private const string AbsoluteExpirationKey = "absexp";
    private const string SlidingExpirationKey = "sldexp";
    private const string DataKey = "data";
    private const long NotPresent = -1;

    /// <summary>
    /// Initializes a new instance of <see cref="RedisCache"/>.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="optionsAccessor">The configuration options.</param>
    /// <param name="logger">The logger.</param>
    internal RedisCache(
        IRedisConnectionMultiplexerFactory connectionFactory,
        IOptions<RedisCacheOptions> optionsAccessor,
        ILogger<RedisCache> logger)
    {
        if (optionsAccessor == null)
        {
            throw new ArgumentNullException(nameof(optionsAccessor));
        }

        _options = optionsAccessor.Value;
        _connectionFactory = connectionFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // This allows partitioning a single backend cache for use with multiple apps/services.
        _instanceName = _options.InstanceName ?? String.Empty;
    }

    #region HashMany

    public byte[][] GetMany(
        IEnumerable<string> keys)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        return this.GetAndRefreshMany(keys, true);
    }

    public async Task<byte[][]> GetManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        return await this.GetAndRefreshManyAsync(keys, true, token);
    }

    public void SetMany(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options)
    {
        this.Connect();

        Task.WaitAll(this.PipelineSetMany(items, options));
    }

    public async Task SetManyAsync(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        await this.ConnectAsync(token);

        await Task.WhenAll(this.PipelineSetMany(items, options));
    }

    public void RefreshMany(
        IEnumerable<string> keys)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        this.GetAndRefreshMany(keys, false);
    }

    public async Task RefreshManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        await this.GetAndRefreshManyAsync(keys, false, token);
    }

    public void RemoveMany(IEnumerable<string> keys)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        this.Connect();

        _cache.KeyDelete(keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray());
    }

    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        keys = Check.IsNotNull(keys, nameof(keys));

        token.ThrowIfCancellationRequested();
        await this.ConnectAsync(token);

        await _cache.KeyDeleteAsync(keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray());
    }

    protected virtual byte[][] GetAndRefreshMany(
        IEnumerable<string> keys,
        bool getData)
    {
        this.Connect();

        var keyArray = keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray();
        RedisValue[][] results;

        if (getData)
        {
            results = _cache.HashMemberGetMany(keyArray, AbsoluteExpirationKey,
                SlidingExpirationKey, DataKey);
        }
        else
        {
            results = _cache.HashMemberGetMany(keyArray, AbsoluteExpirationKey,
                SlidingExpirationKey);
        }

        Task.WaitAll(this.PipelineRefreshManyAndOutData(keyArray, results, out var bytes));

        return bytes;
    }

    protected virtual async Task<byte[][]> GetAndRefreshManyAsync(
        IEnumerable<string> keys,
        bool getData,
        CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        await this.ConnectAsync(token);

        var keyArray = keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray();
        RedisValue[][] results;

        if (getData)
        {
            results = await _cache.HashMemberGetManyAsync(keyArray, AbsoluteExpirationKey,
                SlidingExpirationKey, DataKey);
        }
        else
        {
            results = await _cache.HashMemberGetManyAsync(keyArray, AbsoluteExpirationKey,
                SlidingExpirationKey);
        }

        await Task.WhenAll(this.PipelineRefreshManyAndOutData(keyArray, results, out var bytes));

        return bytes;
    }

    protected virtual Task[] PipelineRefreshManyAndOutData(
        RedisKey[] keys,
        RedisValue[][] results,
        out byte[][] bytes)
    {
        bytes = new byte[keys.Length][];
        var tasks = new Task[keys.Length];

        for (var i = 0; i < keys.Length; i++)
        {
            if (results[i].Length >= 2)
            {
                MapMetadata(results[i], out var absExpr, out var sldExpr);

                if (sldExpr.HasValue)
                {
                    TimeSpan? expr;

                    if (absExpr.HasValue)
                    {
                        var relExpr = absExpr.Value - DateTimeOffset.Now;
                        expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                    }
                    else
                    {
                        expr = sldExpr;
                    }

                    tasks[i] = _cache!.KeyExpireAsync(keys[i], expr);
                }
                else
                {
                    tasks[i] = Task.CompletedTask;
                }
            }

            if (results[i].Length >= 3 && results[i][2].HasValue)
            {
                bytes[i] = results[i][2]!;
            }
            else
            {
                bytes[i] = default!;
            }
        }

        return tasks;
    }

    protected virtual Task[] PipelineSetMany(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options)
    {
        items = Check.IsNotNull(items, nameof(items));
        options = Check.IsNotNull(options, nameof(options));

        var itemArray = items.ToArray();
        var tasks = new Task[itemArray.Length];
        var creationTime = DateTimeOffset.UtcNow;
        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        for (var i = 0; i < itemArray.Length; i++)
        {
            var redisKey = MakeRedisKey(_instanceName, itemArray[i].Key);
            var redisValues = MakeRedisHashSetArgs(itemArray[i].Value, options);

            tasks[i] = _cache!.ScriptEvaluateAsync(HashSetScript, new RedisKey[] { redisKey }, redisValues);
        }

        return tasks;
    }

    #endregion

    #region HashSingle

    public byte[]? Get(string key)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return this.GetAndRefresh(key, getData: true);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        token.ThrowIfCancellationRequested();

        return await this.GetAndRefreshAsync(key, getData: true, token: token).ConfigureAwait(false);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisHashSetArgs(value, options);

        _cache.ScriptEvaluate(HashSetScript, new RedisKey[] { redisKey }, redisValues);
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        token.ThrowIfCancellationRequested();

        await this.ConnectAsync(token).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisHashSetArgs(value, options);

        await _cache.ScriptEvaluateAsync(HashSetScript, new RedisKey[] { redisKey }, redisValues).ConfigureAwait(false);
    }

    public void Refresh(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.GetAndRefresh(key, getData: false);
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        token.ThrowIfCancellationRequested();

        await this.GetAndRefreshAsync(key, getData: false, token: token).ConfigureAwait(false);
    }

    public void Remove(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);

        // bool结果未能返回上层，只能该层日志写出
        if (_cache.KeyDelete(redisKey))
        {
            _logger.LogWarning("The redis key {RedisKey} delete failed.", redisKey);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        await this.ConnectAsync(token).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);

        // bool结果未能返回上层，只能该层日志写出
        if (await _cache.KeyDeleteAsync(redisKey).ConfigureAwait(false))
        {
            _logger.LogWarning("The redis key {RedisKey} delete failed.", redisKey);
        }
    }

    private byte[]? GetAndRefresh(string key, bool getData)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.Connect();

        // This also resets the LRU status as desired.
        // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
        RedisValue[] results;
        var redisKey = MakeRedisKey(_instanceName, key);
        if (getData)
        {
            results = _cache.HashMemberGet(redisKey, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
        }
        else
        {
            results = _cache.HashMemberGet(redisKey, AbsoluteExpirationKey, SlidingExpirationKey);
        }

        // TODO: Error handling
        if (results.Length >= 2)
        {
            MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
            this.Refresh(_cache, key, absExpr, sldExpr);
        }

        if (results.Length >= 3 && results[2].HasValue)
        {
            return results[2];
        }

        return null;
    }

    private async Task<byte[]?> GetAndRefreshAsync(string key, bool getData, CancellationToken token)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        token.ThrowIfCancellationRequested();

        await this.ConnectAsync(token).ConfigureAwait(false);
        Debug.Assert(_cache is not null);

        // This also resets the LRU status as desired.
        // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
        RedisValue[] results;
        if (getData)
        {
            results = await _cache.HashMemberGetAsync(_instanceName + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey).ConfigureAwait(false);
        }
        else
        {
            results = await _cache.HashMemberGetAsync(_instanceName + key, AbsoluteExpirationKey, SlidingExpirationKey).ConfigureAwait(false);
        }

        // TODO: Error handling
        if (results.Length >= 2)
        {
            MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
            await this.RefreshAsync(_cache, key, absExpr, sldExpr, token).ConfigureAwait(false);
        }

        if (results.Length >= 3 && results[2].HasValue)
        {
            return results[2];
        }

        return null;
    }
    private void Refresh(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        // Note Refresh has no effect if there is just an absolute expiration (or neither).
        if (sldExpr.HasValue)
        {
            TimeSpan? expr;
            if (absExpr.HasValue)
            {
                var relExpr = absExpr.Value - DateTimeOffset.Now;
                expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
            }
            else
            {
                expr = sldExpr;
            }

            var redisKey = MakeRedisKey(_instanceName, key);

            if (cache.KeyExpire(redisKey, expr))
            {
                _logger.LogWarning("The redis key {RedisKey} set expire failed.", redisKey);
            }
        }
    }

    private async Task RefreshAsync(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken token)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        token.ThrowIfCancellationRequested();

        // Note Refresh has no effect if there is just an absolute expiration (or neither).
        if (sldExpr.HasValue)
        {
            TimeSpan? expr;
            if (absExpr.HasValue)
            {
                var relExpr = absExpr.Value - DateTimeOffset.Now;
                expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
            }
            else
            {
                expr = sldExpr;
            }

            var redisKey = MakeRedisKey(_instanceName, key);
            if (await cache.KeyExpireAsync(redisKey, expr).ConfigureAwait(false))
            {
                _logger.LogWarning("The redis key {RedisKey} set expire failed.", redisKey);
            }
        }
    }

    #endregion

    #region SortSet

    public bool SortedSetAdd(string key, double order, byte[] value, DistributedCacheEntryOptions options)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisSortedSetAddArgs(order, value, options);

        var result = _cache.ScriptEvaluate(SortedSetAddScript, new RedisKey[] { redisKey }, redisValues);

        return result.Type is not ResultType.Error;
    }

    public async Task<bool> SortedSetAddAsync(string key, double order, byte[] value, DistributedCacheEntryOptions options, CancellationToken token)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        token.ThrowIfCancellationRequested();

        await this.ConnectAsync(token).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisSortedSetAddArgs(order, value, options);

        var result = await _cache.ScriptEvaluateAsync(SortedSetAddScript, new RedisKey[] { redisKey }, redisValues).ConfigureAwait(false);

        return result.Type is not ResultType.Error;
    }

    public long SortedSetGetCount(string key, double? minOrder, double? maxOrder)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);

        return _cache.SortedSetLength(redisKey, minOrder ?? Double.NegativeInfinity, maxOrder ?? Double.PositiveInfinity);
    }
    #endregion

    #region Connect

    [MemberNotNull(nameof(_cache), nameof(_connection))]
    protected void Connect()
    {
        if (_cache is not null)
        {
            return;
        }

        if (_options.Configuration is null)
        {
            throw new InvalidOperationException();
        }

        _connection ??= _connectionFactory.GetConnection(_options.Configuration);

        _cache = _connection.GetDatabase();
    }

    [MemberNotNull(nameof(_cache), nameof(_connection))]
    protected async Task ConnectAsync(CancellationToken cancellation)
    {
        if (_cache is not null)
        {
            return;
        }

        if (_options.Configuration is null)
        {
            throw new InvalidOperationException();
        }

        _connection ??= await _connectionFactory.GetConnectionAsync(_options.Configuration, cancellation);

        _cache = _connection.GetDatabase();
    }

    #endregion

    #region Expiration

    protected static RedisKey MakeRedisKey(string instanceName, string key)
    {
        return instanceName + key;
    }

    protected static RedisValue[] MakeRedisHashSetArgs(byte[] value, DistributedCacheEntryOptions options)
    {
        var creationTime = DateTimeOffset.UtcNow;

        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        var values = new RedisValue[] {
            absoluteExpiration?.UtcTicks ?? NotPresent,
            options.SlidingExpiration?.Ticks ?? NotPresent,
            GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
            value,
        };

        return values;
    }
    protected static RedisValue[] MakeRedisSortedSetAddArgs(double scoren, byte[] value, DistributedCacheEntryOptions options)
    {
        if (options.SlidingExpiration.HasValue)
        {
            throw new NotSupportedException("The redis sortedSet not support the 'SlidingExpiration' setting.");
        }

        var creationTime = DateTimeOffset.UtcNow;

        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        var values = new RedisValue[] {
            GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
            scoren,
            value,
        };

        return values;
    }

    protected static void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
    {
        absoluteExpiration = null;
        slidingExpiration = null;

        var absoluteExpirationTicks = (long?)results[0];
        if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != NotPresent)
        {
            absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
        }

        var slidingExpirationTicks = (long?)results[1];
        if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != NotPresent)
        {
            slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
        }
    }

    private static long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
    {
        if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
        {
            return (long)Math.Min(
                (absoluteExpiration.Value - creationTime).TotalSeconds,
                options.SlidingExpiration.Value.TotalSeconds);
        }
        else if (absoluteExpiration.HasValue)
        {
            return (long)(absoluteExpiration.Value - creationTime).TotalSeconds;
        }
        else if (options.SlidingExpiration.HasValue)
        {
            return (long)options.SlidingExpiration.Value.TotalSeconds;
        }

        return null;
    }

    private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
    {
        if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
        {
            throw new ArgumentOutOfRangeException(
                nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                options.AbsoluteExpiration.Value,
                "The absolute expiration value must be in the future.");
        }

        if (options.AbsoluteExpirationRelativeToNow.HasValue)
        {
            return creationTime + options.AbsoluteExpirationRelativeToNow;
        }

        return options.AbsoluteExpiration;
    }
    #endregion
}
