using System.Reflection;

using FantasySky.CustomDF.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DependencyInjection;

public abstract class ConventionalRegistrarBase : IConventionalRegistrar
{
    public virtual void AddAssembly(IServiceCollection services, Assembly assembly)
    {
        var types = AssemblyHelper
            .GetAllTypes(assembly)
            .Where(
            type =>
                        type != null &&
                        type.IsClass &&
                        !type.IsAbstract &&
                        !type.IsGenericType
            ).ToArray();

        this.AddTypes(services, types);
    }

    public abstract void AddType(IServiceCollection services, Type type);

    public virtual void AddTypes(IServiceCollection services, params Type[] types)
    {
        foreach (var type in types)
        {
            this.AddType(services, type);
        }
    }

    protected virtual ServiceDescriptor CreateServiceDescriptor(
        Type implementationType,
        Type serviceType,
        Type? redirectedType,
        ServiceLifetime lifeTime)
    {
        if (lifeTime is ServiceLifetime.Singleton or ServiceLifetime.Scoped)
        {
            if (redirectedType != null)
            {
                return ServiceDescriptor.Describe(
                    serviceType,
                    provider => provider.GetRequiredService(redirectedType),
                    lifeTime
                );
            }
        }

        return ServiceDescriptor.Describe(
            serviceType,
            implementationType,
            lifeTime
        );
    }

    protected virtual ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return null;
    }

    protected virtual IEnumerable<DependencyAttribute> GetDependencyAttributeOrEmpty(Type type)
    {
        return type.GetCustomAttributes<DependencyAttribute>(true);
    }

    protected virtual ServiceLifetime? GetOtherDependencyLifetime(Type type)
    {
        return this.GetServiceLifetimeFromClassHierarchy(type) ?? this.GetDefaultLifeTimeOrNull(type);
    }

    protected virtual ServiceLifetime? GetServiceLifetimeFromClassHierarchy(Type type)
    {
        //if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
        //{
        //    return ServiceLifetime.Transient;
        //}

        //if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
        //{
        //    return ServiceLifetime.Singleton;
        //}

        //if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
        //{
        //    return ServiceLifetime.Scoped;
        //}

        return null;
    }
}
