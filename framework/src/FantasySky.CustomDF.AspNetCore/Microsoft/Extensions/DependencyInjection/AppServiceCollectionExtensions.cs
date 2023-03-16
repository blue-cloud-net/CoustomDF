namespace Microsoft.Extensions.DependencyInjection;

public static class AppServiceCollectionExtensions
{
    internal static IServiceCollection AddWebAppServices(this IServiceCollection services, Action<IServiceCollection>? configure = null)
    {

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        configure?.Invoke(services);

        return services;
    }
}
