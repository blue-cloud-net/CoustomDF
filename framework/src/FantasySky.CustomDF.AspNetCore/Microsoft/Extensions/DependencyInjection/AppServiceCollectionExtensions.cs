namespace Microsoft.Extensions.DependencyInjection;

public static class AppServiceCollectionExtensions
{
    /// <summary>
    /// 添加Web应用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    internal static IServiceCollection AddWebAppServices(this IServiceCollection services, Action<IServiceCollection>? configure = null)
    {
        services.AddControllers();

        services.AddSwaggerServcie();

        configure?.Invoke(services);

        return services;
    }

    /// <summary>
    /// 添加Swagger服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddSwaggerServcie(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            var xmlDocDir = $"./swagger_doc";
            if (Directory.Exists(xmlDocDir))
            {
                foreach (var xmlDocFilePath in Directory.GetFiles(xmlDocDir, "*.xml", SearchOption.TopDirectoryOnly))
                {
                    options.IncludeXmlComments(xmlDocFilePath, true);
                }
            }
        });

        return services;
    }
}
