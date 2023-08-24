using System.Reflection;

using FantasySky.CustomDF;
using FantasySky.CustomDF.Http.Client;
using FantasySky.CustomDF.Modularity;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class GrpcRemoteExtensions
{
    public static void AddGrpcClientProxies(this ServiceConfigurationContext context, Action<Dictionary<string, Assembly>>? remoteServicesAction = null)
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
                .Where(IsSuitableForClientProxying)
                .ToArray();

            //foreach (var serviceType in serviceTypes)
            //{
            //    services.AddGrpcClient<>();
            //}
        }
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
