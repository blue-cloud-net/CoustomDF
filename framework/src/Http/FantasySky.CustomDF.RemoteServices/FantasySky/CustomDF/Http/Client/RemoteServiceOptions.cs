namespace FantasySky.CustomDF.Http.Client;

public class RemoteServiceOptions
{
    public RemoteServiceOptions()
    {
        this.RemoteServices = new RemoteServiceConfigurationDictionary();
    }

    public RemoteServiceConfigurationDictionary RemoteServices { get; set; }
}
