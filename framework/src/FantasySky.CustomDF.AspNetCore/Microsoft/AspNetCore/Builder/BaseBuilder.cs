using FantasySky.CustomDF.Modularity;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class WebAppBuilder
{
    public static WebApplication CreateAndRunWebApp<Startup>(string[] args,
        Action<ServiceConfigurationContext>? serviceConfigContextAction = null,
        Action<WebApplication>? middlewareSettingAction = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var app = builder.Services.CreateApplication<Startup>(builder.Configuration);

        var serviceConfigContext = new ServiceConfigurationContext(builder.Services, builder.Configuration);

        builder.Services.AddWebAppServices();

        serviceConfigContextAction?.Invoke(serviceConfigContext);

        var webApp = builder.Build();

        middlewareSettingAction?.Invoke(webApp);

        webApp.Run();

        return webApp;
    }
}
