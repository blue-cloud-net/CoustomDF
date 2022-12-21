using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DependencyInjection;

public class DependencyAttribute : Attribute
{
    public virtual ServiceLifetime? Lifetime { get; set; }

    public virtual bool TryRegister { get; set; }

    public virtual bool ReplaceServices { get; set; }

    public DependencyAttribute()
    {

    }

    public DependencyAttribute(ServiceLifetime lifetime)
    {
        this.Lifetime = lifetime;
    }
}