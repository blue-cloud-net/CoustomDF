using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public class ApplicationServiceProvider<Startup> : ApplicationBase<Startup>, IApplicationServiceProvider
{
    public ApplicationServiceProvider(
        IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction) : base(services, configuration, optionsAction)
    {
        services.AddSingleton<IApplicationServiceProvider>(this);
    }
}
