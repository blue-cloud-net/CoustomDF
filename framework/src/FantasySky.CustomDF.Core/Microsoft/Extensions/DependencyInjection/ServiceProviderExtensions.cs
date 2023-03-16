using FantasySky.CustomDF;
using FantasySky.CustomDF.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static T? GetService<T>(this IServiceProvider provider, string serviceName)
        where T : class
    {
        Check.IsNotNull(provider, nameof(provider));
        Check.IsNotNullOrWhiteSpace(serviceName, nameof(serviceName));

        var namedServiceProvider = provider.GetService<INamedServiceProvider<T>>();

        if (namedServiceProvider is null)
        {
            throw new InvalidOperationException($"No type '{typeof(T)}' of named service is registered in container.");
        }

        return namedServiceProvider.GetService(serviceName);
    }

    public static T? GetRequiredService<T>(this IServiceProvider provider, string serviceName)
        where T : class
    {
        Check.IsNotNull(provider, nameof(provider));
        Check.IsNotNullOrWhiteSpace(serviceName, nameof(serviceName));

        var namedServiceProvider = provider.GetService<INamedServiceProvider<T>>();

        if (namedServiceProvider is null)
        {
            throw new InvalidOperationException($"No type '{typeof(T)}' of named service is registered in container.");
        }

        return namedServiceProvider.GetRequiredService(serviceName);
    }
}
