using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Modularity;

public record ServiceConfigurationContext(IServiceCollection Services, IConfiguration Configuration);
