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
    private readonly INamedServiceTypeMap _servicesTypeMap;

    public NamedServiceProvider(
        IServiceProvider serviceProvider,
        INamedServiceTypeMap servicesTypeMap)
    {
        _serviceProvider = serviceProvider;
        _servicesTypeMap = servicesTypeMap;
    }

    public TService GetRequiredService(string serviceName)
    {
        if (_servicesTypeMap.TryGetType<TService>(serviceName, out var type))
        {
            return this.GetRequiredService(type);
        }

        throw new InvalidOperationException($"Named service `{serviceName}` is not registered in container.");
    }

    public TService? GetService(string serviceName)
    {
        if (_servicesTypeMap.TryGetType<TService>(serviceName, out var type))
        {
            return this.GetRequiredService(type);
        }

        return null;
    }

    private TService GetRequiredService(Type implType)
        => _serviceProvider.GetService(implType) as TService
            ?? throw new InvalidOperationException($"Not mathch impl type '{implType.Name}' in interface type '{typeof(TService).Name}'.");
}
