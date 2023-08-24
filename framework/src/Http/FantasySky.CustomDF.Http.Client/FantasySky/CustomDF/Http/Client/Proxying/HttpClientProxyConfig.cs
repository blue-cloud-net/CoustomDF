namespace FantasySky.CustomDF.Http.Client.Proxying;

public class HttpClientProxyConfig
{
    public HttpClientProxyConfig(Type type, string remoteServiceName)
    {
        this.Type = type;
        this.RemoteServiceName = remoteServiceName;
    }

    public Type Type { get; }

    public string RemoteServiceName { get; }
}
