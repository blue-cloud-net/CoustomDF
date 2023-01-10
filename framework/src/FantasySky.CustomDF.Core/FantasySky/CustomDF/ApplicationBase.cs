using System.Reflection;

using FantasySky.CustomDF.Exceptions;
using FantasySky.CustomDF.Internal;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public abstract class ApplicationBase : IApplication
{
    public Type StartupType { get; }

    public IServiceProvider? ServiceProvider { get; private set; }

    public IServiceCollection Services { get; }

    public IConfiguration Configuration { get; }

    public string? ApplicationName { get; }

    public string InstanceId { get; } = Guid.NewGuid().ToString();

    private bool _configuredServices;

    internal ApplicationBase(
        Type startupType,
        IServiceCollection services,
        IConfiguration configuration,
        Action<ApplicationCreationOptions>? optionsAction)
    {
        Check.IsNotNull(startupType, nameof(startupType));
        Check.IsNotNull(services, nameof(services));

        this.StartupType = startupType;
        this.Services = services;
        this.Configuration = configuration;

        var options = new ApplicationCreationOptions(services);
        optionsAction?.Invoke(options);

        this.ApplicationName = this.GetApplicationName(options);

        services.AddSingleton<IApplication>(this);
        services.AddSingleton<IApplicationInfoAccessor>(this);

        services.AddCoreServices();
        services.AddCoreAbpServices(this, options);

        if (!options.SkipConfigureServices)
        {
            this.ConfigureServices();
        }
    }

    public virtual void ConfigureServices()
    {
        this.CheckMultipleConfigureServices();

        _configuredServices = true;

        throw new NotImplementedException();
    }

    public virtual void Dispose()
    {
        //TODO: Shutdown if not done before?
    }

    public Task ShutdownAsync()
    {
        throw new NotImplementedException();
    }

    public void Shutdown()
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
