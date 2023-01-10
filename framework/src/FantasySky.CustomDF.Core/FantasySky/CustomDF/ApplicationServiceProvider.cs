using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public class ApplicationServiceProvider : ApplicationBase, IApplicationServiceProvider
{
    public ApplicationServiceProvider(
        Type startupType,
        IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction) : base(startupType, services, configuration, optionsAction)
    {
        services.AddSingleton<IApplicationServiceProvider>(this);
    }
}
