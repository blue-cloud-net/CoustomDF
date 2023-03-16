namespace FantasySky.CustomDF.DependencyInjection;

/// <summary>
/// 命名服务提供器
/// </summary>
/// <typeparam name="TService">目标服务接口</typeparam>
public interface INamedServiceProvider<TService>
    where TService : class
{
    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService? GetService(string serviceName);

    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService GetRequiredService(string serviceName);
}
