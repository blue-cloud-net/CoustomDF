using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FantasySky.CustomDF.Internal;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddCoreServices(this IServiceCollection services)
    {
        // TODO

        //services.AddOptions();
        //services.AddLogging();
        //services.AddLocalization();
    }

    internal static void AddCoreAbpServices(this IServiceCollection services,
        IApplication abpApplication,
        ApplicationCreationOptions applicationCreationOptions)
    {
        // TODO

        services.AddAssemblyOf<IApplication>();
    }
}
