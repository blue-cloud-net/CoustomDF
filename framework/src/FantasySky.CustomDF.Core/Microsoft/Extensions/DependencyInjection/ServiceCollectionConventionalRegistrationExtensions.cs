using System.Reflection;

using FantasySky.CustomDF.DependencyInjection;
using FantasySky.CustomDF.Exceptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConventionalRegistrationExtensions
{
    public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddAssembly(services, assembly);
        }

        return services;
    }

    public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
    {
        // TODO 没搞明白Abp怎么优化自动DI
        var startupAssembly = typeof(T).GetTypeInfo().Assembly;

        var dllDirectoryPath = Path.GetDirectoryName(startupAssembly.Location)
            ?? throw new FrameworkException("Can not find the directory of entry assembly.");

        var referencedAssemblies = new List<Assembly>(50);

        var dllPaths = Directory.GetFiles(dllDirectoryPath, "*.dll");

        foreach (var dllPath in dllPaths)
        {
            referencedAssemblies.Add(Assembly.LoadFrom(dllPath));
        }

        //var referencedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var referencedAssembly in referencedAssemblies)
        {
            services.AddAssembly(referencedAssembly);
        }
        //services.AddAssembly();

        return services;
    }

    public static List<IConventionalRegistrar> GetConventionalRegistrars(this IServiceCollection services)
    {
        return GetOrCreateRegistrarList();
    }

    private static List<IConventionalRegistrar> GetOrCreateRegistrarList()
    {
        var conventionalRegistrars = new List<IConventionalRegistrar> { new DefaultConventionalRegistrar() };

        return conventionalRegistrars;
    }
}
