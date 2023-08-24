using FantasySky.CustomDF.Http.Client.Proxying;

namespace FantasySky.CustomDF.Http.Client;

internal class HttpClientOptions
{
    public HttpClientOptions()
    {
        this.HttpClientProxies = new Dictionary<Type, HttpClientProxyConfig>();
    }

    public Dictionary<Type, HttpClientProxyConfig> HttpClientProxies { get; set; }
}
