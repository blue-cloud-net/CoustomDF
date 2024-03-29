using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;

public interface IApplication :
    IApplicationInfoAccessor,
    IDisposable
{
    /// <summary>
    /// Reference to the root service provider used by the application.
    /// This can not be used before initialize the application.
    /// </summary>
    IServiceProvider? ServiceProvider { get; }

    /// <summary>
    /// List of services registered to this application.
    /// Can not add new services to this collection after application initialize.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Type of the startup (entrance) module of the application.
    /// </summary>
    Type StartupType { get; }

    /// <summary>
    /// Used to gracefully shutdown the application and all modules.
    /// </summary>
    void Shutdown();

    /// <summary>
    /// Used to gracefully shutdown the application and all modules.
    /// </summary>
    Task ShutdownAsync();
}
