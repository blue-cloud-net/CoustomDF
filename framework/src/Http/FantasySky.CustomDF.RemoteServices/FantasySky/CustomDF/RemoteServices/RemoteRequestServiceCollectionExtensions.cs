//using System.Reflection;

//using FantasySky.CustomDF.HttpProxying;

//using Microsoft.Extensions.DependencyInjection;

//namespace FantasySky.CustomDF.RemoteServices;

//public static class RemoteRequestServiceCollectionExtensions
//{
//    public static IServiceCollection AddRemoteService(this IServiceCollection services, Action<IServiceCollection> configure = null, bool inludeDefaultHttpClient = true)
//    {
//        // 注册远程Http请求(默认带有)代理接口
//        services.AddScopedDispatchProxyForInterface<HttpDispatchProxy, IHttpDispatchProxy>();

//        // 注册默认请求客户端
//        if (inludeDefaultHttpClient)
//        {
//            services.AddHttpClient();
//        }

//        // 注册其他客户端
//        configure?.Invoke(services);

//        return services;
//    }

//    /// <summary>
//    /// 添加接口代理
//    /// </summary>
//    /// <typeparam name="TDispatchProxy">代理类</typeparam>
//    /// <typeparam name="TIDispatchProxy">被代理接口依赖</typeparam>
//    /// <param name="services">服务集合</param>
//    /// <returns>服务集合</returns>
//    public static IServiceCollection AddScopedDispatchProxyForInterface<TDispatchProxy, TIDispatchProxy>(this IServiceCollection services)
//        where TDispatchProxy : AspectDispatchProxy, IDispatchProxy
//        where TIDispatchProxy : class
//    {
//        // 注册代理类
//        services.AddScoped<AspectDispatchProxy, TDispatchProxy>();

//        // 代理依赖接口类型
//        var proxyType = typeof(TDispatchProxy);
//        var typeDependency = typeof(TIDispatchProxy);

//        // 获取所有的代理接口类型
//        var dispatchProxyInterfaceTypes = App.EffectiveTypes
//            .Where(u => typeDependency.IsAssignableFrom(u) && u.IsInterface && u != typeDependency);

//        // 注册代理类型
//        foreach (var interfaceType in dispatchProxyInterfaceTypes)
//        {
//            AddDispatchProxy(services, typeof(), default, proxyType, interfaceType, false);
//        }

//        return services;
//    }

//    /// <summary>
//    /// 创建服务代理
//    /// </summary>
//    /// <param name="services">服务集合</param>
//    /// <param name="dependencyType"></param>
//    /// <param name="type">拦截的类型</param>
//    /// <param name="proxyType">代理类型</param>
//    /// <param name="inter">代理接口</param>
//    /// <param name="hasTarget">是否有实现类</param>
//    private static void AddRemoteServiceProxy(IServiceCollection services, Type dependencyType, Type type, Type proxyType, Type inter, bool hasTarget = true)
//    {
//        proxyType ??= GlobalServiceProxyType;
//        if (proxyType == null || (type != null && type.IsDefined(typeof(SuppressProxyAttribute), true)))
//            return;

//        // 注册代理类型
//        services.InnerAdd(dependencyType, typeof(AspectDispatchProxy), proxyType);

//        // 注册服务
//        services.InnerAdd(dependencyType, inter, provider =>
//        {
//            dynamic proxy = DispatchCreateMethod.MakeGenericMethod(inter, proxyType).Invoke(null, null);
//            proxy.Services = provider;
//            if (hasTarget)
//            {
//                proxy.Target = provider.GetService(type);
//            }

//            return proxy;
//        });
//    }
//}
