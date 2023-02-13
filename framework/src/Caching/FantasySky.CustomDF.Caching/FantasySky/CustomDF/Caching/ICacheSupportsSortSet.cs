using Microsoft.Extensions.Caching.Distributed;

namespace FantasySky.CustomDF.Caching;

public interface ICacheSupportsSortSet
{
    bool SortedSetAdd(string key, double order, byte[] value, DistributedCacheEntryOptions? options = null);

    Task<bool> SortedSetAddAsync(string key, double order, byte[] value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default);

    long SortedSetGetCount(string key, double minOrder, double maxOrder);

    Task<long> SortedSetGetCountAsync(string key, double minOrder, double maxOrder, CancellationToken cancellation = default);
}
