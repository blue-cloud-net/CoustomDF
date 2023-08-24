using System.Reflection;

using FantasySky.CustomDF;
using FantasySky.CustomDF.Http.Client;
using FantasySky.CustomDF.Modularity;

using Microsoft.Extensions.Configuration;

using Refit;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClientExtensions
{
    public static void AddHttpClientProxies(this ServiceConfigurationContext context, Action<Dictionary<string, Assembly>>? remoteServicesAction = null)
    {
        var services = context.Services;

        var remoteServices = new Dictionary<string, Assembly>(10);
        remoteServicesAction?.Invoke(remoteServices);

        services.Configure<RemoteServiceOptions>(context.Configuration);
        var options = context.Configuration.Get<RemoteServiceOptions>() ?? new();

        foreach (var (remoteServiceName, assembly) in remoteServices)
        {
            var remoteServiceConfig = options.RemoteServices.GetConfiguration(remoteServiceName);

            var serviceTypes = assembly
                .GetTypes()
                .Where(x => IsSuitableForClientProxying(x))
                .ToArray();

            foreach (var serviceType in serviceTypes)
            {
                services.AddHttpClientProxy(
                    serviceType,
                    remoteServiceConfig
                );
            }
        }
    }

    public static void AddHttpClientProxies(
        this ServiceConfigurationContext context,
        Assembly assembly,
        string remoteServiceConfigurationName)
    {
        var services = context.Services;

        Check.NotNull(services, nameof(assembly));
        Check.NotNull(assembly, nameof(assembly));
        Check.NotNullOrWhiteSpace(remoteServiceConfigurationName, nameof(remoteServiceConfigurationName));

        services.ConfigureOptions<RemoteServiceOptions>();
        var options = context.Configuration.Get<RemoteServiceOptions>() ?? new();
        var remoteServiceConfig = options.RemoteServices.GetConfiguration(remoteServiceConfigurationName);

        var serviceTypes = assembly
            .GetTypes()
            .Where(x => IsSuitableForClientProxying(x))
            .ToArray();

        foreach (var serviceType in serviceTypes)
        {
            services.AddHttpClientProxy(
                serviceType,
                remoteServiceConfig
            );
        }
    }

    public static void AddHttpClientProxy(
        this IServiceCollection services,
        Type type,
        RemoteServiceConfiguration remoteServiceConfig)
    {
        //var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        //jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        //jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        ////jsonSerializerOptions.Converters.Add(new CancellationTokenJsonConverter());

        //var config = new RefitSettings()
        //{
        //    ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions)
        //};

        // TODO

        services
            .AddRefitClient(type)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(remoteServiceConfig.BaseUrl));
    }

    private static bool IsSuitableForClientProxying(Type type)
    {
        if (!type.IsInterface ||
            !type.IsPublic ||
            type.IsGenericType ||
            !typeof(IRemoteService).IsAssignableFrom(type))
        {
            return false;
        }

        return true;
    }
}
