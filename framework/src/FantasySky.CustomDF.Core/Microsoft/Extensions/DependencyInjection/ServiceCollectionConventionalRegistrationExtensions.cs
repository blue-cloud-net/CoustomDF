using System.Reflection;

using FantasySky.CustomDF.DependencyInjection;

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
        return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
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
