namespace FantasySky.CustomDF.Caching;

public class DistributedCacheKeyNormalizeArgs
{
    public string Key { get; }

    public string CacheName { get; }

    public DistributedCacheKeyNormalizeArgs(
        string key,
        string cacheName)
    {
        this.Key = key;
        this.CacheName = cacheName;
    }
}
