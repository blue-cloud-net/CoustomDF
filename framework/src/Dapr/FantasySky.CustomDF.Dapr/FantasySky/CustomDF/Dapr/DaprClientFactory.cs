using Dapr.Client;

using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Dapr;

public class DaprClientFactory : IDaprClientFactory
{
    protected DaprOptions DaprOptions { get; }

    public DaprClientFactory(
        IOptions<DaprOptions> options)
    {
        this.DaprOptions = options.Value;
    }

    public DaprClient Create(Action<DaprClientBuilder>? builderAction = null)
    {
        var builder = new DaprClientBuilder();

        if (!this.DaprOptions.HttpEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseHttpEndpoint(this.DaprOptions.HttpEndpoint);
        }

        if (!this.DaprOptions.GrpcEndpoint.IsNullOrWhiteSpace())
        {
            builder.UseGrpcEndpoint(this.DaprOptions.GrpcEndpoint);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }

    public HttpClient CreateHttpClient(string? appId = null, string? daprEndpoint = null, string? daprApiToken = null)
    {
        throw new NotImplementedException();
    }
}
