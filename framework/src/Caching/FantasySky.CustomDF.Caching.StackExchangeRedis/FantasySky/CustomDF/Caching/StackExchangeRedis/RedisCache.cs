using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

/// <summary>
///
/// </summary>
public class RedisCache :
    IDistributedCache,
    ICacheSupportsDeleteReturnResult,
    ICacheSupportsMultipleItems,
    ICacheSupportsSortSet,
    ICacheSupportsList,
    ICacheSupportsIncrement
{
    private const string _absoluteExpirationKey = "absexp";

    private const string _dataKey = "data";

    private const string _hashSetScript = @"
                redis.call('HSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                    redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1";

    private const long _notPresent = -1;

    private const string _slidingExpirationKey = "sldexp";

    private const string _sortedSetAddScript = @"
                redis.call('ZADD', KEYS[1], ARGV[2], ARGV[3])
                if ARGV[1] ~= '-1' then
                    redis.call('EXPIRE', KEYS[1], ARGV[1])
                end
                return 1";

    private readonly IRedisConnectionMultiplexerFactory _connectionFactory;
    private readonly string _instanceName;
    private readonly ILogger _logger;
    private readonly RedisCacheOptions _options;
    private IDatabase? _cache;
    private volatile IConnectionMultiplexer? _connection;

    /// <summary>
    /// Initializes a new instance of <see cref="RedisCache"/>.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="optionsAccessor">The configuration options.</param>
    /// <param name="logger">The logger.</param>
    public RedisCache(
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
        keys = Check.NotNull(keys, nameof(keys));

        return this.GetAndRefreshMany(keys, true);
    }

    public async Task<byte[][]> GetManyAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        return await this.GetAndRefreshManyAsync(keys, true, cancellationToken);
    }

    public void RefreshMany(
        IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        this.GetAndRefreshMany(keys, false);
    }

    public async Task RefreshManyAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        await this.GetAndRefreshManyAsync(keys, false, cancellationToken);
    }

    public void RemoveMany(IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        this.Connect();

        _cache.KeyDelete(keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray());
    }

    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        cancellationToken.ThrowIfCancellationRequested();
        await this.ConnectAsync(cancellationToken);

        await _cache.KeyDeleteAsync(keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray());
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
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        await Task.WhenAll(this.PipelineSetMany(items, options));
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
            results = _cache.HashMemberGetMany(keyArray, _absoluteExpirationKey,
                _slidingExpirationKey, _dataKey);
        }
        else
        {
            results = _cache.HashMemberGetMany(keyArray, _absoluteExpirationKey,
                _slidingExpirationKey);
        }

        Task.WaitAll(this.PipelineRefreshManyAndOutData(keyArray, results, out var bytes));

        return bytes;
    }

    protected virtual async Task<byte[][]> GetAndRefreshManyAsync(
        IEnumerable<string> keys,
        bool getData,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var keyArray = keys.Select(key => MakeRedisKey(_instanceName, key)).ToArray();
        RedisValue[][] results;

        if (getData)
        {
            results = await _cache.HashMemberGetManyAsync(keyArray, _absoluteExpirationKey,
                _slidingExpirationKey, _dataKey);
        }
        else
        {
            results = await _cache.HashMemberGetManyAsync(keyArray, _absoluteExpirationKey,
                _slidingExpirationKey);
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
                        var relExpr = absExpr.Value - DateTimeOffset.UtcNow;
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
        items = Check.NotNull(items, nameof(items));
        options = Check.NotNull(options, nameof(options));

        var itemArray = items.ToArray();
        var tasks = new Task[itemArray.Length];
        var creationTime = DateTimeOffset.UtcNow;
        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        for (var i = 0; i < itemArray.Length; i++)
        {
            var redisKey = MakeRedisKey(_instanceName, itemArray[i].Key);
            var redisValues = MakeRedisHashSetArgs(itemArray[i].Value, options);

            tasks[i] = _cache!.ScriptEvaluateAsync(_hashSetScript, new RedisKey[] { redisKey }, redisValues);
        }

        return tasks;
    }

    #endregion HashMany

    #region HashSingle

    public byte[]? Get(string key)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        return this.GetAndRefresh(key, getData: true);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        if (String.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        cancellationToken.ThrowIfCancellationRequested();

        return await this.GetAndRefreshAsync(key, getData: true, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public void Refresh(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        this.GetAndRefresh(key, getData: false);
    }

    public async Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        cancellationToken.ThrowIfCancellationRequested();

        await this.GetAndRefreshAsync(key, getData: false, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public void Remove(string key)
        => this.RemoveWithResult(key);

    public bool RemoveWithResult(string key)
    {
        Check.NotNull(key, nameof(key));

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);

        return _cache.KeyDelete(redisKey);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await this.RemoveWithResultAsync(key, cancellationToken);

    public async Task<bool> RemoveWithResultAsync(string key, CancellationToken cancellationToken = default)
    {
        Check.NotNull(key, nameof(key));

        await this.ConnectAsync(cancellationToken).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);

        return await _cache.KeyDeleteAsync(redisKey).ConfigureAwait(false);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        Check.NotNull(key, nameof(key));
        Check.NotNull(value, nameof(value));
        Check.NotNull(options, nameof(options));

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisHashSetArgs(value, options);

        _cache.ScriptEvaluate(_hashSetScript, new RedisKey[] { redisKey }, redisValues);
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
    {
        Check.NotNull(key, nameof(key));
        Check.NotNull(value, nameof(value));
        Check.NotNull(options, nameof(options));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisHashSetArgs(value, options);

        await _cache.ScriptEvaluateAsync(_hashSetScript, new RedisKey[] { redisKey }, redisValues).ConfigureAwait(false);
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
            results = _cache.HashMemberGet(redisKey, _absoluteExpirationKey, _slidingExpirationKey, _dataKey);
        }
        else
        {
            results = _cache.HashMemberGet(redisKey, _absoluteExpirationKey, _slidingExpirationKey);
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

    private async Task<byte[]?> GetAndRefreshAsync(string key, bool getData, CancellationToken cancellationToken)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken).ConfigureAwait(false);
        Debug.Assert(_cache is not null);

        // This also resets the LRU status as desired.
        // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
        RedisValue[] results;
        if (getData)
        {
            results = await _cache.HashMemberGetAsync(_instanceName + key, _absoluteExpirationKey, _slidingExpirationKey, _dataKey).ConfigureAwait(false);
        }
        else
        {
            results = await _cache.HashMemberGetAsync(_instanceName + key, _absoluteExpirationKey, _slidingExpirationKey).ConfigureAwait(false);
        }

        // TODO: Error handling
        if (results.Length >= 2)
        {
            MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
            await this.RefreshAsync(_cache, key, absExpr, sldExpr, cancellationToken).ConfigureAwait(false);
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
                var relExpr = absExpr.Value - DateTimeOffset.UtcNow;
                expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
            }
            else
            {
                expr = sldExpr;
            }

            var redisKey = MakeRedisKey(_instanceName, key);

            if (!cache.KeyExpire(redisKey, expr))
            {
                _logger.LogWarning("The redis key {RedisKey} set expire failed.", redisKey);
            }
        }
    }

    private async Task RefreshAsync(IDatabase cache, string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken cancellationToken)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        cancellationToken.ThrowIfCancellationRequested();

        // Note Refresh has no effect if there is just an absolute expiration (or neither).
        if (sldExpr.HasValue)
        {
            TimeSpan? expr;
            if (absExpr.HasValue)
            {
                var relExpr = absExpr.Value - DateTimeOffset.UtcNow;
                expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
            }
            else
            {
                expr = sldExpr;
            }

            var redisKey = MakeRedisKey(_instanceName, key);
            if (!await cache.KeyExpireAsync(redisKey, expr).ConfigureAwait(false))
            {
                _logger.LogWarning("The redis key {RedisKey} set expire failed.", redisKey);
            }
        }
    }

    #endregion HashSingle

    #region SortedSet

    public bool SortedSetAdd(string key, double order, byte[] value, DistributedCacheEntryOptions? options = null)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(value, nameof(value));
        options ??= new DistributedCacheEntryOptions();

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisSortedSetAddArgs(order, value, options);

        var result = _cache.ScriptEvaluate(_sortedSetAddScript, new RedisKey[] { redisKey }, redisValues);

        return result.Type is not ResultType.Error;
    }

    public async Task<bool> SortedSetAddAsync(string key, double order, byte[] value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(value, nameof(value));
        options ??= new DistributedCacheEntryOptions();

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);
        var redisValues = MakeRedisSortedSetAddArgs(order, value, options);

        var result = await _cache.ScriptEvaluateAsync(_sortedSetAddScript, new RedisKey[] { redisKey }, redisValues).ConfigureAwait(false);

        return result.Type is not ResultType.Error;
    }

    public async Task<bool> SortedSetRemoveAsync(string key, byte[] value, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(value, nameof(value));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken).ConfigureAwait(false);

        var redisKey = MakeRedisKey(_instanceName, key);

        await _cache.SortedSetRemoveAsync(redisKey, value);

        return true;
    }

    public long SortedSetGetCount(string key, double minOrder, double maxOrder)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        this.Connect();

        var redisKey = MakeRedisKey(_instanceName, key);

        return _cache.SortedSetLength(redisKey, minOrder, maxOrder);
    }

    public async Task<long> SortedSetGetCountAsync(string key, double minOrder, double maxOrder, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        return await _cache.SortedSetLengthAsync(redisKey, minOrder, maxOrder);
    }

    public async Task<byte[][]> SortedSetListAsync(string key, double minOrder, double maxOrder, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        var valus = await _cache.SortedSetRangeByScoreAsync(redisKey, minOrder, maxOrder);

        return valus.Select(p => (byte[])p!).ToArray();
    }

    #endregion SortSet

    #region List

    public async Task<bool> ListLPushAsync(
        string key,
        byte[] value,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        await _cache.ListLeftPushAsync(redisKey, value);

        return true;
    }

    public async Task<byte[]?> ListLPopAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        return await _cache.ListLeftPopAsync(redisKey);
    }

    public async Task<bool> ListRPushAsync(
        string key,
        byte[] value,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        await _cache.ListRightPushAsync(redisKey, value);

        return true;
    }

    public async Task<byte[]?> ListRPopAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        return await _cache.ListRightPopAsync(redisKey);
    }

    public async Task<byte[][]> ListRangeAsync(
        string key,
        long? min = null,
        long? max = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        min ??= 0;
        max ??= -1;

        if (min < 0)
        {
            throw new ArgumentNullException(nameof(min));
        }

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        var values = await _cache.ListRangeAsync(redisKey, min.Value, max.Value);

        return values.Select(p => (byte[])p!).ToArray();
    }

    public async Task<long> ListCountAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var redisKey = MakeRedisKey(_instanceName, key);

        var count = await _cache.ListLengthAsync(redisKey);

        return count;
    }

    #endregion

    public async Task<long> IncrementAsync(string key, string filed, long increment = 1, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        cancellationToken.ThrowIfCancellationRequested();

        await this.ConnectAsync(cancellationToken);

        var result = await _cache.HashIncrementAsync(key, filed, increment);

        return result;
    }

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
    protected async Task ConnectAsync(CancellationToken cancellationToken)
    {
        if (_cache is not null)
        {
            return;
        }

        if (_options.Configuration is null)
        {
            throw new InvalidOperationException();
        }

        _connection ??= await _connectionFactory.GetConnectionAsync(_options.Configuration, cancellationToken);

        _cache = _connection.GetDatabase();
    }

    #endregion Connect

    #region Expiration

    protected static RedisValue[] MakeRedisHashSetArgs(byte[] value, DistributedCacheEntryOptions options)
    {
        var creationTime = DateTimeOffset.UtcNow;

        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        var values = new RedisValue[] {
            absoluteExpiration?.UtcTicks ?? _notPresent,
            options.SlidingExpiration?.Ticks ?? _notPresent,
            GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? _notPresent,
            value,
        };

        return values;
    }

    protected static RedisKey MakeRedisKey(string instanceName, string key)
    {
        return instanceName + key;
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
            GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? _notPresent,
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
        if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != _notPresent)
        {
            absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
        }

        var slidingExpirationTicks = (long?)results[1];
        if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != _notPresent)
        {
            slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
        }
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

    #endregion Expiration
}
