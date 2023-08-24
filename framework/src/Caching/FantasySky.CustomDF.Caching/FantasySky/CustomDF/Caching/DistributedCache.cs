using System.Diagnostics.CodeAnalysis;

using FantasySky.CustomDF.Exceptions;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Caching;

public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
{
    public IDistributedCache<TCacheItem, string> InternalCache { get; }

    public DistributedCache(IDistributedCache<TCacheItem, string> internalCache)
    {
        this.InternalCache = internalCache;
    }

    #region Basic

    public TCacheItem? Get(string key)
        => this.InternalCache.Get(key);

    public KeyValuePair<string, TCacheItem?>[] GetMany(IEnumerable<string> keys)
        => this.InternalCache.GetMany(keys);

    public Task<KeyValuePair<string, TCacheItem?>[]> GetManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        => this.InternalCache.GetManyAsync(keys, cancellationToken);

    public Task<TCacheItem?> GetAsync(string key, CancellationToken cancellationToken = default)
        => this.InternalCache.GetAsync(key, cancellationToken);

    public TCacheItem? GetOrAdd(string key, Func<TCacheItem> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null)
        => this.InternalCache.GetOrAdd(key, factory, optionsFactory);

    public Task<TCacheItem?> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, CancellationToken cancellationToken = default)
        => this.InternalCache.GetOrAddAsync(key, factory, optionsFactory, cancellationToken);

    public KeyValuePair<string, TCacheItem?>[] GetOrAddMany(IEnumerable<string> keys, Func<IEnumerable<string>, List<KeyValuePair<string, TCacheItem>>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null)
        => this.InternalCache.GetOrAddMany(keys, factory, optionsFactory);

    public Task<KeyValuePair<string, TCacheItem?>[]> GetOrAddManyAsync(IEnumerable<string> keys, Func<IEnumerable<string>, Task<List<KeyValuePair<string, TCacheItem>>>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, CancellationToken cancellationToken = default)
        => this.InternalCache.GetOrAddManyAsync(keys, factory, optionsFactory, cancellationToken);

    public void Set(string key, TCacheItem value, DistributedCacheEntryOptions? options = null)
    {
        this.InternalCache.Set(key, value, options);
    }

    public Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
        => this.InternalCache.SetAsync(key, value, options, cancellationToken);

    public void SetMany(IEnumerable<KeyValuePair<string, TCacheItem>> items, DistributedCacheEntryOptions? options = null)
    {
        this.InternalCache.SetMany(items, options);
    }

    public Task SetManyAsync(IEnumerable<KeyValuePair<string, TCacheItem>> items, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
        => this.InternalCache.SetManyAsync(items, options, cancellationToken);

    public void Refresh(string key)
    {
        this.InternalCache.Refresh(key);
    }

    public Task RefreshAsync(string key, CancellationToken cancellationToken = default)
        => this.InternalCache.RefreshAsync(key, cancellationToken);

    public void RefreshMany(IEnumerable<string> keys)
    {
        this.InternalCache.RefreshMany(keys);
    }

    public Task RefreshManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        => this.InternalCache.RefreshManyAsync(keys, cancellationToken);

    public bool Remove(string key)
        => this.InternalCache.Remove(key);

    public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        => this.InternalCache.RemoveAsync(key, cancellationToken);

    public bool RemoveMany(IEnumerable<string> keys)
        => this.InternalCache.RemoveMany(keys);

    public Task<bool> RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        => this.InternalCache.RemoveManyAsync(keys, cancellationToken);

    #endregion

    #region SortedSet

    public Task<bool> SortedSetAddAsync(string key, double orderNumber, TCacheItem value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
        => this.InternalCache.SortedSetAddAsync(key, orderNumber, value, options, cancellationToken);

    public Task<bool> SortedSetRemoveAsync(string key, TCacheItem value, CancellationToken cancellationToken = default)
        => this.InternalCache.SortedSetRemoveAsync(key, value, cancellationToken);

    public Task<long> SortedSetCountAsync(string key, double? min = null, double? max = null, CancellationToken cancellationToken = default)
        => this.InternalCache.SortedSetCountAsync(key, min, max, cancellationToken);

    public Task<List<TCacheItem>> SortedSetListAsync(string key, double? min = null, double? max = null, CancellationToken cancellationToken = default)
        => this.InternalCache.SortedSetListAsync(key, min, max, cancellationToken);

    #endregion

    #region List

    public Task<bool> ListLPushAsync(string key, TCacheItem value, CancellationToken cancellationToken = default)
        => this.InternalCache.ListLPushAsync(key, value, cancellationToken);

    public Task<TCacheItem?> ListLPopAsync(string key, CancellationToken cancellationToken = default)
        => this.InternalCache.ListRPopAsync(key, cancellationToken);

    public Task<bool> ListRPushAsync(string key, TCacheItem value, CancellationToken cancellationToken = default)
        => this.InternalCache.ListRPushAsync(key, value, cancellationToken);

    public Task<TCacheItem?> ListRPopAsync(string key, CancellationToken cancellationToken = default)
    => this.InternalCache.ListRPopAsync(key, cancellationToken);

    public Task<List<TCacheItem>> ListRangeAsync(string key, int? min = null, int? max = null, CancellationToken cancellationToken = default)
        => this.InternalCache.ListRangeAsync(key, min, max, cancellationToken);

    public Task<long> ListCountAsync(string key, CancellationToken cancellationToken = default)
        => this.InternalCache.ListCountAsync(key, cancellationToken);

    #endregion
}

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// Uses a generic cache key type of <typeparamref name="TCacheKey" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public class DistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
{
    public ILogger<DistributedCache<TCacheItem, TCacheKey>> Logger { get; set; }

    protected string CacheName { get; set; } = String.Empty;

    protected bool IgnoreMultiTenancy { get; set; }

    protected IDistributedCache Cache { get; }

    protected IDistributedCacheSerializer Serializer { get; }

    protected IDistributedCacheKeyNormalizer KeyNormalizer { get; }

    protected SemaphoreSlim SyncSemaphore { get; }

    protected DistributedCacheEntryOptions DefaultCacheOptions;

    private readonly DistributedCacheOptions _distributedCacheOption;

    public DistributedCache(
        IOptions<DistributedCacheOptions> distributedCacheOption,
        ILogger<DistributedCache<TCacheItem, TCacheKey>> logger,
        IDistributedCache cache,
        IDistributedCacheSerializer serializer,
        IDistributedCacheKeyNormalizer keyNormalizer)
    {
        _distributedCacheOption = distributedCacheOption.Value;
        this.Cache = cache;
        this.Logger = logger;
        this.Serializer = serializer;
        this.KeyNormalizer = keyNormalizer;

        this.SyncSemaphore = new SemaphoreSlim(1, 1);

        this.SetDefaultOptions();
    }

    protected virtual DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        return _distributedCacheOption.GlobalCacheEntryOptions;
    }

    [MemberNotNull(nameof(DefaultCacheOptions))]
    protected virtual void SetDefaultOptions()
    {
        this.CacheName = CacheNameAttribute.GetCacheName(typeof(TCacheItem));

        //Configure default cache entry options
        this.DefaultCacheOptions = this.GetDefaultCacheEntryOptions();
    }

    protected virtual string NormalizeKey(TCacheKey key)
    {
        Check.NotNull(key, nameof(key));

        if (key is string keyString)
        {
            return keyString;
        }

        return this.KeyNormalizer.NormalizeKey(
            new DistributedCacheKeyNormalizeArgs(
                key.ToString() ?? String.Empty,
                this.CacheName
            )
        );
    }

    #region Basic

    public virtual TCacheItem? Get(TCacheKey key)
    {
        byte[]? cachedBytes;

        try
        {
            cachedBytes = this.Cache.Get(this.NormalizeKey(key));
        }
        catch (Exception ex)
        {
            this.HandleException(ex);

            return default;
        }

        return cachedBytes is null ? default : this.ToCacheItem(cachedBytes);
    }

    public virtual KeyValuePair<TCacheKey, TCacheItem?>[] GetMany(
                IEnumerable<TCacheKey> keys)
    {
        var keyArray = keys.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            return this.GetManyFallback(
                keyArray
            );
        }

        byte[][] cachedBytes;

        try
        {
            cachedBytes = cacheSupportsMultipleItems.GetMany(keyArray.Select(this.NormalizeKey));
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return ToCacheItemsWithDefaultValues(keyArray);
        }

        return this.ToCacheItems(cachedBytes, keyArray);
    }

    protected virtual KeyValuePair<TCacheKey, TCacheItem?>[] GetManyFallback(
                TCacheKey[] keys)
    {
        try
        {
            return keys
                .Select(key => new KeyValuePair<TCacheKey, TCacheItem?>(
                        key,
                        this.Get(key)
                    )
                ).ToArray();
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return ToCacheItemsWithDefaultValues(keys);
        }
    }

    public virtual async Task<KeyValuePair<TCacheKey, TCacheItem?>[]> GetManyAsync(
                IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default)
    {
        var keyArray = keys.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            return await this.GetManyFallbackAsync(
                keyArray,
                cancellationToken
            );
        }

        byte[][] cachedBytes;

        try
        {
            cachedBytes = await cacheSupportsMultipleItems.GetManyAsync(
                keyArray.Select(this.NormalizeKey),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return ToCacheItemsWithDefaultValues(keyArray);
        }

        return this.ToCacheItems(cachedBytes, keyArray);
    }

    protected virtual async Task<KeyValuePair<TCacheKey, TCacheItem?>[]> GetManyFallbackAsync(
                TCacheKey[] keys,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = new List<KeyValuePair<TCacheKey, TCacheItem?>>();

            foreach (var key in keys)
            {
                result.Add(new KeyValuePair<TCacheKey, TCacheItem?>(
                    key,
                    await this.GetAsync(key, cancellationToken))
                );
            }

            return result.ToArray();
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return ToCacheItemsWithDefaultValues(keys);
        }
    }

    public virtual async Task<TCacheItem?> GetAsync(
                TCacheKey key,
        CancellationToken cancellationToken = default)
    {
        byte[]? cachedBytes;

        try
        {
            cachedBytes = await this.Cache.GetAsync(
                this.NormalizeKey(key),
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return default;
        }

        if (cachedBytes is null)
        {
            return default;
        }

        return this.Serializer.Deserialize<TCacheItem>(cachedBytes);
    }

    public virtual TCacheItem? GetOrAdd(
                TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null)
    {
        var value = this.Get(key);
        if (value != null)
        {
            return value;
        }

        this.SyncSemaphore.Wait();

        try
        {
            value = this.Get(key);
            if (value != null)
            {
                return value;
            }

            value = factory();

            this.Set(key, value, optionsFactory?.Invoke());
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
        }
        finally
        {
            this.SyncSemaphore.Release();
        }

        return value;
    }

    public virtual async Task<TCacheItem?> GetOrAddAsync(
                TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default)
    {
        var value = await this.GetAsync(key, cancellationToken);
        if (value != null)
        {
            return value;
        }

        this.SyncSemaphore.Wait();

        try
        {
            value = await this.GetAsync(key, cancellationToken);
            if (value != null)
            {
                return value;
            }

            value = await factory();

            await this.SetAsync(key, value, optionsFactory?.Invoke(), cancellationToken);
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
        }
        finally
        {
            this.SyncSemaphore.Release();
        }

        return value;
    }

    public KeyValuePair<TCacheKey, TCacheItem?>[] GetOrAddMany(
                IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, List<KeyValuePair<TCacheKey, TCacheItem>>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null)
    {
        KeyValuePair<TCacheKey, TCacheItem?>[] result;
        var keyArray = keys.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            result = this.GetManyFallback(
                keyArray
            );
        }
        else
        {
            byte[][] cachedBytes;

            try
            {
                cachedBytes = cacheSupportsMultipleItems.GetMany(keys.Select(this.NormalizeKey));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                return ToCacheItemsWithDefaultValues(keyArray);
            }

            result = this.ToCacheItems(cachedBytes, keys.ToArray());
        }

        if (result.All(x => x.Value != null))
        {
            return result;
        }

        var missingKeys = new List<TCacheKey>();
        var missingValuesIndex = new List<int>();
        for (var i = 0; i < keyArray.Length; i++)
        {
            if (result[i].Value != null)
            {
                continue;
            }

            missingKeys.Add(keyArray[i]);
            missingValuesIndex.Add(i);
        }

        var missingValues = factory.Invoke(missingKeys).ToArray();
        var valueQueue = new Queue<KeyValuePair<TCacheKey, TCacheItem>>(missingValues);

        this.SetMany(missingValues, optionsFactory?.Invoke());

        foreach (var index in missingValuesIndex)
        {
            result[index] = valueQueue.Dequeue()!;
        }

        return result;
    }

    public async Task<KeyValuePair<TCacheKey, TCacheItem?>[]> GetOrAddManyAsync(
                IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, Task<List<KeyValuePair<TCacheKey, TCacheItem>>>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default)
    {
        KeyValuePair<TCacheKey, TCacheItem?>[] result;
        var keyArray = keys.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            result = await this.GetManyFallbackAsync(
                keyArray,
                cancellationToken);
        }
        else
        {
            byte[][] cachedBytes;

            try
            {
                cachedBytes = await cacheSupportsMultipleItems.GetManyAsync(keys.Select(this.NormalizeKey), cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return ToCacheItemsWithDefaultValues(keyArray);
            }

            result = this.ToCacheItems(cachedBytes, keyArray);
        }

        if (result.All(x => x.Value != null))
        {
            return result;
        }

        var missingKeys = new List<TCacheKey>();
        var missingValuesIndex = new List<int>();
        for (var i = 0; i < keyArray.Length; i++)
        {
            if (result[i].Value != null)
            {
                continue;
            }

            missingKeys.Add(keyArray[i]);
            missingValuesIndex.Add(i);
        }

        var missingValues = (await factory.Invoke(missingKeys)).ToArray();
        var valueQueue = new Queue<KeyValuePair<TCacheKey, TCacheItem>>(missingValues);

        await this.SetManyAsync(missingValues, optionsFactory?.Invoke(), cancellationToken);

        foreach (var index in missingValuesIndex)
        {
            result[index] = valueQueue.Dequeue()!;
        }

        return result;
    }

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    public virtual void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null)
    {
        try
        {
            this.Cache.Set(
                this.NormalizeKey(key),
                this.Serializer.Serialize(value),
                options ?? DefaultCacheOptions
            );
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return;
        }
    }

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await this.Cache.SetAsync(
                this.NormalizeKey(key),
                this.Serializer.Serialize(value),
                options ?? DefaultCacheOptions,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return;
        }
    }

    public void SetMany(
            IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions? options = null)
    {
        var itemsArray = items.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            this.SetManyFallback(
                itemsArray,
                options
            );

            return;
        }

        try
        {
            cacheSupportsMultipleItems.SetMany(
                this.ToRawCacheItems(itemsArray),
                options ?? DefaultCacheOptions
            );
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return;
        }
    }

    protected virtual void SetManyFallback(
            KeyValuePair<TCacheKey, TCacheItem>[] items,
        DistributedCacheEntryOptions? options = null)
    {
        try
        {
            foreach (var item in items)
            {
                this.Set(
                    item.Key,
                    item.Value,
                    options
                );
            }
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return;
        }
    }

    public virtual async Task SetManyAsync(
            IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var itemsArray = items.ToArray();

        if (this.Cache is not ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            await this.SetManyFallbackAsync(
                itemsArray,
                options,
                cancellationToken
            );

            return;
        }

        try
        {
            await cacheSupportsMultipleItems.SetManyAsync(
                this.ToRawCacheItems(itemsArray),
                options ?? DefaultCacheOptions,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return;
        }
    }

    protected virtual async Task SetManyFallbackAsync(
            KeyValuePair<TCacheKey, TCacheItem>[] items,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in items)
            {
                await this.SetAsync(
                    item.Key,
                    item.Value,
                    options,
                    cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return;
        }
    }

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    public virtual void Refresh(
        TCacheKey key)
    {
        try
        {
            this.Cache.Refresh(this.NormalizeKey(key));
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return;
        }
    }

    public virtual async Task RefreshAsync(
            TCacheKey key,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await this.Cache.RefreshAsync(this.NormalizeKey(key), cancellationToken);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return;
        }
    }

    public virtual void RefreshMany(
            IEnumerable<TCacheKey> keys)
    {
        try
        {
            if (this.Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
            {
                cacheSupportsMultipleItems.RefreshMany(keys.Select(this.NormalizeKey));
            }
            else
            {
                foreach (var key in keys)
                {
                    this.Cache.Refresh(this.NormalizeKey(key));
                }
            }
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return;
        }
    }

    public virtual async Task RefreshManyAsync(
            IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (this.Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
            {
                await cacheSupportsMultipleItems.RefreshManyAsync(keys.Select(this.NormalizeKey), cancellationToken);
            }
            else
            {
                foreach (var key in keys)
                {
                    await this.Cache.RefreshAsync(this.NormalizeKey(key), cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return;
        }
    }

    public virtual bool Remove(
            TCacheKey key)
    {
        try
        {
            var normalizeKey = this.NormalizeKey(key);

            if (this.Cache is ICacheSupportsDeleteReturnResult cacheWithResult)
            {
                return cacheWithResult.RemoveWithResult(normalizeKey);
            }

            this.Cache.Remove(normalizeKey);
        }
        catch (Exception ex)
        {
            this.HandleException(ex);
            return false;
        }

        return true;
    }

    public virtual async Task<bool> RemoveAsync(
            TCacheKey key,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var normalizeKey = this.NormalizeKey(key);

            if (this.Cache is ICacheSupportsDeleteReturnResult cacheWithResult)
            {
                return await cacheWithResult.RemoveWithResultAsync(normalizeKey);
            }

            await this.Cache.RemoveAsync(this.NormalizeKey(key), cancellationToken);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(ex);
            return false;
        }

        return true;
    }

    public bool RemoveMany(
            IEnumerable<TCacheKey> keys)
    {
        var keyArray = keys.ToArray();

        if (this.Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            try
            {
                cacheSupportsMultipleItems.RemoveMany(
                    keyArray.Select(this.NormalizeKey)
                );
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                return false;
            }
        }
        else
        {
            foreach (var key in keyArray)
            {
                this.Remove(key);
            }
        }

        return true;
    }

    public async Task<bool> RemoveManyAsync(
            IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default)
    {
        var keyArray = keys.ToArray();

        if (this.Cache is ICacheSupportsMultipleItems cacheSupportsMultipleItems)
        {
            try
            {
                await cacheSupportsMultipleItems.RemoveManyAsync(
                    keyArray.Select(this.NormalizeKey), cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return false;
            }
        }
        else
        {
            foreach (var key in keyArray)
            {
                await this.RemoveAsync(key, cancellationToken);
            }
        }

        return true;
    }

    #endregion

    #region SortSet

    public async Task<bool> SortedSetAddAsync(TCacheKey key, double orderNumber, TCacheItem value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsSortSet sortCache)
        {
            try
            {
                return await sortCache.SortedSetAddAsync(
                    this.NormalizeKey(key),
                    orderNumber,
                    this.Serializer.Serialize(value),
                    options,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return false;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<bool> SortedSetRemoveAsync(TCacheKey key, TCacheItem value, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsSortSet sortCache)
        {
            try
            {
                return await sortCache.SortedSetRemoveAsync(
                    this.NormalizeKey(key),
                    this.Serializer.Serialize(value),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return false;
            }
        }

        throw new NotSupportedException();
    }

    public Task<long> SortedSetCountAsync(TCacheKey key, double? min = null, double? max = null, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsSortSet sortCache)
        {
            return sortCache.SortedSetGetCountAsync(
                this.NormalizeKey(key),
                min ?? Double.MinValue,
                max ?? Double.MaxValue,
                cancellationToken);
        }

        throw new NotSupportedException();
    }

    public async Task<List<TCacheItem>> SortedSetListAsync(TCacheKey key, double? min = null, double? max = null, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsSortSet sortCache)
        {
            var values = await sortCache.SortedSetListAsync(
                this.NormalizeKey(key),
                min ?? Double.MinValue,
                max ?? Double.MaxValue,
                cancellationToken);

            return values.Select(p => this.Serializer.Deserialize<TCacheItem>(p)!).ToList();
        }

        throw new NotSupportedException();
    }

    #endregion

    #region List

    public async Task<bool> ListLPushAsync(TCacheKey key, TCacheItem value, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                return await listCache.ListLPushAsync(
                    this.NormalizeKey(key),
                    this.Serializer.Serialize(value),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return false;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<TCacheItem?> ListLPopAsync(TCacheKey key, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                var cachedBytes = await listCache.ListLPopAsync(
                    this.NormalizeKey(key),
                    cancellationToken);

                return this.ToCacheItem(cachedBytes);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return default;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<bool> ListRPushAsync(TCacheKey key, TCacheItem value, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                return await listCache.ListRPushAsync(
                    this.NormalizeKey(key),
                    this.Serializer.Serialize(value),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return false;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<TCacheItem?> ListRPopAsync(TCacheKey key, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                var cachedBytes = await listCache.ListRPopAsync(
                    this.NormalizeKey(key),
                    cancellationToken);

                return this.ToCacheItem(cachedBytes);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return default;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<List<TCacheItem>> ListRangeAsync(TCacheKey key, int? min = null, int? max = null, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                var cachedBytesArray = await listCache.ListRangeAsync(
                    this.NormalizeKey(key),
                    min,
                    max,
                    cancellationToken);

                var cacheItems = cachedBytesArray.Select(p => this.ToCacheItem(p)!).ToList();

                return cacheItems;
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return new List<TCacheItem>(0);
            }
        }

        throw new NotSupportedException();
    }

    public async Task<long> ListCountAsync(TCacheKey key, CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsList listCache)
        {
            try
            {
                var count = await listCache.ListCountAsync(
                    this.NormalizeKey(key),
                    cancellationToken);

                return count;
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);
                return 0;
            }
        }

        throw new NotSupportedException();
    }

    #endregion

    protected virtual void HandleException(Exception ex)
    {
        _ = this.HandleExceptionAsync(ex);
    }

    protected virtual ValueTask HandleExceptionAsync(Exception ex)
    {
        this.Logger.LogError(ex, "An exception occurred in the distributed cache operation.");

        return ValueTask.CompletedTask;
    }

    protected virtual KeyValuePair<TCacheKey, TCacheItem?>[] ToCacheItems(byte[][] itemBytes, TCacheKey[] itemKeys)
    {
        if (itemBytes.Length != itemKeys.Length)
        {
            throw new FrameworkException("count of the item bytes should be same with the count of the given keys");
        }

        var result = new List<KeyValuePair<TCacheKey, TCacheItem?>>();

        for (int i = 0; i < itemKeys.Length; i++)
        {
            result.Add(
                new KeyValuePair<TCacheKey, TCacheItem?>(
                    itemKeys[i],
                    this.ToCacheItem(itemBytes[i])
                )
            );
        }

        return result.ToArray();
    }

    protected virtual TCacheItem? ToCacheItem(byte[]? bytes)
    {
        if (bytes == null)
        {
            return default;
        }

        return this.Serializer.Deserialize<TCacheItem>(bytes);
    }

    protected virtual KeyValuePair<string, byte[]>[] ToRawCacheItems(KeyValuePair<TCacheKey, TCacheItem>[] items)
    {
        return items
            .Select(i => new KeyValuePair<string, byte[]>(
                    this.NormalizeKey(i.Key),
                    this.Serializer.Serialize(i.Value)
                )
            ).ToArray();
    }

    private static KeyValuePair<TCacheKey, TCacheItem?>[] ToCacheItemsWithDefaultValues(TCacheKey[] keys)
    {
        return keys
            .Select(key => new KeyValuePair<TCacheKey, TCacheItem?>(key, default))
            .ToArray();
    }
}
