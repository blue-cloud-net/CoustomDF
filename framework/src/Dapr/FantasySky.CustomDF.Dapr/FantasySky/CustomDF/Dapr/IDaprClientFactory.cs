using Dapr.Client;

namespace FantasySky.CustomDF.Dapr;

public interface IDaprClientFactory
{
    DaprClient Create(Action<DaprClientBuilder> builderAction = null);

    HttpClient CreateHttpClient(
        string appId = null,
        string daprEndpoint = null,
        string daprApiToken = null
    );
}
