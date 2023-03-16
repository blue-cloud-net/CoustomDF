using FantasySky.CustomDF.Caching.StackExchangeRedis;
using FantasySky.CustomDF.Exceptions;
using FantasySky.CustomDF.Modularity;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionRedisCacheExtensions
{

    public static void AddRedisDistributedCache(this ServiceConfigurationContext context)
    {
        context.AddDistributedCache();

        var redisCacheOptions = context.Configuration.GetValue<RedisCacheOptions>(RedisCacheOptions.Path);

        context.Services.Configure<RedisCacheOptions>(
            options => context.Configuration.GetValue<RedisCacheOptions>(RedisCacheOptions.Path));

        if (redisCacheOptions?.IsEnabled is true)
        {
            context.Services.AddStackExchangeRedisCache(options =>
            {
                if (!String.IsNullOrWhiteSpace(redisCacheOptions?.Configuration))
                {
                    options.Configuration = redisCacheOptions?.Configuration;
                }

                throw new FrameworkException("Can not read the redis connection string.");
            });

            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
        }

        return;
    }
}
