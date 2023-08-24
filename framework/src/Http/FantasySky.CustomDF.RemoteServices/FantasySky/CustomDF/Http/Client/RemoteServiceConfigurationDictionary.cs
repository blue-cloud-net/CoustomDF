using FantasySky.CustomDF.Exceptions;

namespace FantasySky.CustomDF.Http.Client;

public class RemoteServiceConfigurationDictionary : Dictionary<string, RemoteServiceConfiguration>
{
    public const string DefaultName = "Default";

    public RemoteServiceConfiguration Default
    {
        get => this.GetOrDefault(DefaultName) ?? new RemoteServiceConfiguration() { BaseUrl = "https://localhost" };
        set => this[DefaultName] = value;
    }

    public RemoteServiceConfiguration GetConfiguration(string name)
        => this.GetValueOrDefault(name)
        ?? throw new FrameworkException($"Remote service '{name}' was not found and there is no default configuration.");

    public RemoteServiceConfiguration GetConfigurationOrDefault(string name)
        => this.GetValueOrDefault(name) ?? this.Default;
}
