using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF;
public class ApplicationCreationOptions
{
    [NotNull]
    public IServiceCollection Services { get; }

    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    //[NotNull]
    //public ConfigurationBuilderOptions Configuration { get; }

    public bool SkipConfigureServices { get; set; }

    public string? ApplicationName { get; set; }

    public ApplicationCreationOptions([NotNull] IServiceCollection services)
    {
        this.Services = Check.IsNotNull(services, nameof(services));

        //this.Configuration = new ConfigurationBuilderOptions();
    }
}
