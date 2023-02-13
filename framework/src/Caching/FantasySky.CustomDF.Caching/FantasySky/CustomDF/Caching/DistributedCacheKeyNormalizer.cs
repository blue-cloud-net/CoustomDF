using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Caching;

public class DistributedCacheKeyNormalizer
{
    //protected ICurrentTenant CurrentTenant { get; }

    protected DistributedCacheOptions DistributedCacheOptions { get; }

    public DistributedCacheKeyNormalizer(
        //ICurrentTenant currentTenant,
        IOptions<DistributedCacheOptions> distributedCacheOptions)
    {
        //CurrentTenant = currentTenant;
        this.DistributedCacheOptions = distributedCacheOptions.Value;
    }

    public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        var normalizedKey = $"c:{args.CacheName},k:{this.DistributedCacheOptions.KeyPrefix}{args.Key}";

        //if (!args.IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
        //{
        //    normalizedKey = $"t:{CurrentTenant.Id.Value},{normalizedKey}";
        //}

        return normalizedKey;
    }
}
