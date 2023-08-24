using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Internal;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddAppCoreServices<Startup>(this IServiceCollection services,
        IApplication application,
        ApplicationCreationOptions applicationCreationOptions)
    {
        // 注册命名服务
        services.AddScoped(typeof(INamedServiceProvider<>), typeof(NamedServiceProvider<>));

        services.AddAssemblyOf<Startup>();
    }

    internal static void AddCoreServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddLogging();

        // TODO
        //services.AddLocalization();
    }
}
