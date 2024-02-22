using FantasySky.CustomDF;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static IApplicationServiceProvider CreateApplication<Startup>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        return ApplicationFactory.Create<Startup>(services, configuration, optionsAction);
    }
}
