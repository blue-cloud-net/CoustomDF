using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FantasySky.CustomDF.DependencyInjection;

public class DefaultConventionalRegistrar : ConventionalRegistrarBase
{
    public override void AddType(IServiceCollection services, Type type)
    {
        // TODO Disabled Dependency Inject
        //if (base.IsConventionalRegistrationDisabled(type))
        //{
        //    return;
        //}

        // Attribute inject
        var dependencyAttributes = base.GetDependencyAttributeOrEmpty(type);

        if (!dependencyAttributes.Any())
        {
            // other inject (interface etc.)
            var lifeTime = base.GetOtherDependencyLifetime(type);

            if (lifeTime is not null)
            {
                services.Add(new(type, lifeTime));

                return;
            }

            return;
        }

        Type? redirectedType = null;

        foreach (var dependencyAttribute in dependencyAttributes)
        {
            var serviceDescriptor = base.CreateServiceDescriptor(
                type,
                // named service 以原类型注册
                dependencyAttribute.ServiceName.IsNullOrWhiteSpace() ?
                dependencyAttribute.InterfaceType ?? type : type,
                redirectedType,
                dependencyAttribute.Lifetime
            );

            // named servcie
            if (!dependencyAttribute.ServiceName.IsNullOrWhiteSpace())
            {
                services.AddNameService(
                    dependencyAttribute.InterfaceType ?? type,
                    type,
                    dependencyAttribute.ServiceName);
            }

            if (dependencyAttribute?.ReplaceServices == true)
            {
                services.Replace(serviceDescriptor);
            }
            else if (dependencyAttribute?.TryRegister == true)
            {
                services.TryAdd(serviceDescriptor);
            }
            else
            {
                services.Add(serviceDescriptor);
            }

            redirectedType ??= serviceDescriptor.ServiceType;
        }
    }
}
