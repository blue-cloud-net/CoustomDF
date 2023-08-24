using FantasySky.CustomDF.DistributedLocking;
using FantasySky.CustomDF.Modularity;

using Medallion.Threading;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionDistributedLockExtensions
{
    public static void AddDistributedLock(this ServiceConfigurationContext context,
        Func<DistributedLockOptions, IServiceProvider, IDistributedLockProvider> lockerProviderFactory)
    {
        var redisSelection = context.Configuration.GetSection(DistributedLockOptions.Path);

        var distributedLockOptions = new DistributedLockOptions();

        redisSelection.Bind(distributedLockOptions);
        context.Services.Configure<DistributedLockOptions>(context.Configuration.GetSection(DistributedLockOptions.Path));

        context.Services.AddSingleton(serviceProvider =>
        {
            var options = distributedLockOptions;
            var lockProvider = lockerProviderFactory(options, serviceProvider);

            return lockProvider;
        });

        return;
    }
}
