using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public static class ApplicationFactory
{
    public static IApplicationServiceProvider Create<TStartup>(
        IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        return new ApplicationServiceProvider<TStartup>(services, configuration, optionsAction);
    }
}
