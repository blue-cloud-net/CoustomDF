using Microsoft.Extensions.Caching.Distributed;

namespace FantasySky.CustomDF.Caching;

public interface ICacheSupportsSortSet
{
    bool SortedSetAdd(string key, double order, byte[] value, DistributedCacheEntryOptions options);
}
