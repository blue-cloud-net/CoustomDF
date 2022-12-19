using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;
public interface IApplication :
    IApplicationInfoAccessor,
    IDisposable
{
    /// <summary>
    /// Type of the startup (entrance) module of the application.
    /// </summary>
    Type StartupType { get; }

    /// <summary>
    /// List of services registered to this application.
    /// Can not add new services to this collection after application initialize.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Reference to the root service provider used by the application.
    /// This can not be used before initialize the application.
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Used to gracefully shutdown the application and all modules.
    /// </summary>
    Task ShutdownAsync();

    /// <summary>
    /// Used to gracefully shutdown the application and all modules.
    /// </summary>
    void Shutdown();
}
