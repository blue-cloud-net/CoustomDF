using System.Reflection;

namespace FantasySky.CustomDF.HttpProxying;

public class HttpDispatchProxy
{
    ///// <summary>
    ///// 拦截同步方法
    ///// </summary>
    ///// <param name="method"></param>
    ///// <param name="args"></param>
    ///// <returns></returns>
    //public override object Invoke(MethodInfo method, object[] args)
    //{
    //    throw new NotSupportedException("Please use asynchronous operation mode.");
    //}

    ///// <summary>
    ///// 拦截异步无返回方法
    ///// </summary>
    ///// <param name="method"></param>
    ///// <param name="args"></param>
    ///// <returns></returns>
    //public async override Task InvokeAsync(MethodInfo method, object[] args)
    //{
    //    var httpRequestPart = BuildHttpRequestPart(method, args);
    //    _ = await httpRequestPart.SendAsync();
    //}

    ///// <summary>
    ///// 拦截异步带返回方法
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="method"></param>
    ///// <param name="args"></param>
    ///// <returns></returns>
    //public override Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
    //{
    //    var httpRequestPart = BuildHttpRequestPart(method, args);
    //    var result = httpRequestPart.SendAsAsync<T>();
    //    return result;
    //}
}
