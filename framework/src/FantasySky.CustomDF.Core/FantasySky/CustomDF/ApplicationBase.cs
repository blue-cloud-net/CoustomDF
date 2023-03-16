using System.Reflection;

using FantasySky.CustomDF.Exceptions;
using FantasySky.CustomDF.Internal;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public abstract class ApplicationBase<Startup> : IApplication
{
    private bool _configuredServices;

    internal ApplicationBase(
        IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction)
    {
        Check.IsNotNull(services, nameof(services));

        this.StartupType = typeof(Startup);
        this.Services = services;
        this.Configuration = configuration;

        var options = new ApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        this.ApplicationName = this.GetApplicationName(options);

        services.AddSingleton<IApplication>(this);
        services.AddSingleton<IApplicationInfoAccessor>(this);

        services.AddCoreServices();
        services.AddAppCoreServices<Startup>(this, options);

        if (!options.SkipConfigureServices)
        {
            this.ConfigureServices();
        }
    }

    public string? ApplicationName { get; }
    public IConfiguration Configuration { get; }
    public string InstanceId { get; } = Guid.NewGuid().ToString();
    public IServiceProvider? ServiceProvider { get; private set; }
    public IServiceCollection Services { get; }
    public Type StartupType { get; }

    public virtual void ConfigureServices()
    {
        this.CheckMultipleConfigureServices();

        _configuredServices = true;
    }

    public virtual void Dispose()
    {
        //TODO: Shutdown if not done before?
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }

    public Task ShutdownAsync()
    {
        throw new NotImplementedException();
    }

    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new InitializationException("Services have already been configured! If you call ConfigureServicesAsync method, you must have set AbpApplicationCreationOptions.SkipConfigureServices to true before.");
        }
    }

    private string? GetApplicationName(ApplicationCreationOptions options)
    {
        if (!String.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return options.ApplicationName;
        }

        var appNameConfig = this.Configuration["ApplicationName"];
        if (!String.IsNullOrWhiteSpace(appNameConfig))
        {
            return appNameConfig;
        }

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            return entryAssembly.GetName().Name;
        }

        return null;
    }
}
