namespace FantasySky.CustomDF.Http.Client.Proxying;

public class DefaultProxyHttpClientFactory : IProxyHttpClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultProxyHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient Create()
    {
        return _httpClientFactory.CreateClient();
    }

    public HttpClient Create(string name)
    {
        return _httpClientFactory.CreateClient(name);
    }
}
