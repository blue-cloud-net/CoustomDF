using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DependencyInjection;

/// <summary>
/// 命名服务提供器默认实现
/// </summary>
/// <typeparam name="TService">目标服务接口</typeparam>

public class NamedServiceProvider<TService> : INamedServiceProvider<TService>
    where TService : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, Type> _servicesTypeMap;

    public NamedServiceProvider(
        IServiceProvider serviceProvider,
        IDictionary<string, Type> servicesTypeMap)
    {
        _serviceProvider = serviceProvider;
        _servicesTypeMap = servicesTypeMap;
    }

    public TService GetRequiredService(string serviceName)
    {
        if (_servicesTypeMap.TryGetValue(serviceName, out var type))
        {
            return _serviceProvider.GetRequiredService<TService>();
        }

        throw new InvalidOperationException($"Named service `{serviceName}` is not registered in container.");
    }

    public TService? GetService(string serviceName)
    {
        if (_servicesTypeMap.TryGetValue(serviceName, out var type))
        {
            return _serviceProvider.GetRequiredService<TService>();
        }

        return null;
    }
}
