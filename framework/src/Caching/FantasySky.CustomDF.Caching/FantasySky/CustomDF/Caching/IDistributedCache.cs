using Microsoft.Extensions.Caching.Distributed;

namespace FantasySky.CustomDF.Caching;

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
public interface IDistributedCache<TCacheItem> : IDistributedCache<TCacheItem, string>
{
    IDistributedCache<TCacheItem, string> InternalCache { get; }
}

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// Uses a generic cache key type of <typeparamref name="TCacheKey" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public interface IDistributedCache<TCacheItem, TCacheKey>
{
    #region Basic

    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <returns>The cache item, or null.</returns>
    TCacheItem? Get(
        TCacheKey key
    );

    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item, or null.</returns>
    Task<TCacheItem?> GetAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets multiple cache items with the given keys.
    ///
    /// The returned list contains exactly the same count of items specified in the given keys.
    /// An item in the return list can not be null, but an item in the list has null value
    /// if the related key not found in the cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <returns>List of cache items.</returns>
    KeyValuePair<TCacheKey, TCacheItem?>[] GetMany(
        IEnumerable<TCacheKey> keys
    );

    /// <summary>
    /// Gets multiple cache items with the given keys.
    ///
    /// The returned list contains exactly the same count of items specified in the given keys.
    /// An item in the return list can not be null, but an item in the list has null value
    /// if the related key not found in the cache.
    ///
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>List of cache items.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem?>[]> GetManyAsync(
        IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <returns>The cache item.</returns>
    TCacheItem? GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null
    );

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item.</returns>
    Task<TCacheItem?> GetOrAddAsync(
        TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets or Adds multiple cache items with the given keys. If any cache items not found for the given keys then adds cache items
    /// provided by <paramref name="factory" /> delegate and returns the provided cache items.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache items when no cache items are found for the given <paramref name="keys" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <returns>The cache items.</returns>
    KeyValuePair<TCacheKey, TCacheItem?>[] GetOrAddMany(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, List<KeyValuePair<TCacheKey, TCacheItem>>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null
    );

    /// <summary>
    /// Gets or Adds multiple cache items with the given keys. If any cache items not found for the given keys then adds cache items
    /// provided by <paramref name="factory" /> delegate and returns the provided cache items.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache items when no cache items are found for the given <paramref name="keys" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache items.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem?>[]> GetOrAddManyAsync(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, Task<List<KeyValuePair<TCacheKey, TCacheItem>>>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    void Refresh(
        TCacheKey key
    );

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RefreshAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Refreshes the cache value of the given keys, and resets their sliding expiration timeout.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    void RefreshMany(
        IEnumerable<TCacheKey> keys);

    /// <summary>
    /// Refreshes the cache value of the given keys, and resets their sliding expiration timeout.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RefreshManyAsync(
        IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    bool Remove(
        TCacheKey key
    );

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task<bool> RemoveAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Removes the cache items for given keys from cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    bool RemoveMany(
        IEnumerable<TCacheKey> keys
    );

    /// <summary>
    /// Removes the cache items for given keys from cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task<bool> RemoveManyAsync(
        IEnumerable<TCacheKey> keys,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null
    );

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sets multiple cache items.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="items">Items to set on the cache</param>
    /// <param name="options">The cache options for the value.</param>
    void SetMany(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions? options = null
    );

    /// <summary>
    /// Sets multiple cache items.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="items">Items to set on the cache</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task SetManyAsync(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default
    );

    #endregion Basic

    #region SortedSet

    Task<bool> SortedSetAddAsync(
        TCacheKey key,
        double orderNumber,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<bool> SortedSetRemoveAsync(
        TCacheKey key,
        TCacheItem value,
        CancellationToken cancellationToken = default);

    Task<List<TCacheItem>> SortedSetListAsync(
        TCacheKey key,
        double? min = null,
        double? max = null,
        CancellationToken cancellationToken = default);

    Task<long> SortedSetCountAsync(
        TCacheKey key,
        double? min = null,
        double? max = null,
        CancellationToken cancellationToken = default);

    #endregion SortedSet

    #region List

    Task<bool> ListLPushAsync(
        TCacheKey key,
        TCacheItem value,
        CancellationToken cancellationToken = default);

    Task<TCacheItem?> ListLPopAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default);

    Task<bool> ListRPushAsync(
        TCacheKey key,
        TCacheItem value,
        CancellationToken cancellationToken = default);

    Task<TCacheItem?> ListRPopAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default);

    Task<List<TCacheItem>> ListRangeAsync(
        TCacheKey key,
        int? min = null,
        int? max = null,
        CancellationToken cancellationToken = default);

    Task<long> ListCountAsync(
        TCacheKey key,
        CancellationToken cancellationToken = default);

    #endregion
}
