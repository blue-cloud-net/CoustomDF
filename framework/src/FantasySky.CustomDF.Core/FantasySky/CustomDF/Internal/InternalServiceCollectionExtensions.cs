using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Internal;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddCoreAbpServices(this IServiceCollection services,
        IApplication abpApplication,
        ApplicationCreationOptions applicationCreationOptions)
    {
        // TODO

        services.AddAssemblyOf<IApplication>();
    }

    internal static void AddCoreServices(this IServiceCollection services)
    {
        // TODO

        //services.AddOptions();
        //services.AddLogging();
        //services.AddLocalization();
    }
}
