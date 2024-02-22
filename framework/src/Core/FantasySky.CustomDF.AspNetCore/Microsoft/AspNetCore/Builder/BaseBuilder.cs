using FantasySky.CustomDF.BackgroundJob;
using FantasySky.CustomDF.Modularity;
using FantasySky.CustomDF.StartupTask;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

public static class WebAppBuilder
{
    public static WebApplication CreateAndRunWebApp<Startup>(string[] args,
        Action<ServiceConfigurationWebContext>? serviceConfigContextAction = null,
        Action<WebApplication>? middlewareSettingAction = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var app = builder.Services.CreateApplication<Startup>(builder.Configuration);

        var serviceConfigContext = new ServiceConfigurationWebContext(builder.Services, builder.Configuration, builder.Host);

        builder.Services.AddWebAppServices();

        builder.Services.AddHostedService<StartupRunnerHostedService>();
        builder.Services.AddHostedService<BackgroundJobHostedService>();

        serviceConfigContextAction?.Invoke(serviceConfigContext);

        var webApp = builder.Build();

        middlewareSettingAction?.Invoke(webApp);

        webApp.Run();

        return webApp;
    }
}
