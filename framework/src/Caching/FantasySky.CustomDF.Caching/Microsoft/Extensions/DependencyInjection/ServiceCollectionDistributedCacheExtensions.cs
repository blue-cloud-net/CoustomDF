using FantasySky.CustomDF.Caching;
using FantasySky.CustomDF.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionDistributedCacheExtensions
{
    public static void AddDistributedCache(this ServiceConfigurationContext context)
    {
        context.Services.Configure<DistributedCacheOptions>(context.Configuration.GetSection(DistributedCacheOptions.Path));

        context.Services.AddMemoryCache();
        context.Services.AddDistributedMemoryCache();

        context.Services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        context.Services.AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>));

        context.Services.AddSingleton(typeof(INoIncrementer), typeof(NoIncrementer));

        return;
    }
}
