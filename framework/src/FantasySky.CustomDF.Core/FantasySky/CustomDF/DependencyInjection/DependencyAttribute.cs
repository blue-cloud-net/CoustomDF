using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependencyAttribute : Attribute
{
    public DependencyAttribute()
    {
        this.Lifetime = ServiceLifetime.Scoped;
    }

    public DependencyAttribute(ServiceLifetime lifetime)
    {
        this.Lifetime = lifetime;
    }

    public DependencyAttribute(Type interfaceType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        this.InterfaceType = interfaceType;
        this.Lifetime = lifetime;
    }

    public virtual Type? InterfaceType { get; set; }
    public virtual ServiceLifetime Lifetime { get; set; }
    public virtual bool ReplaceServices { get; set; }
    public virtual bool TryRegister { get; set; }

    public virtual string? ServiceName { get; set; }
}
