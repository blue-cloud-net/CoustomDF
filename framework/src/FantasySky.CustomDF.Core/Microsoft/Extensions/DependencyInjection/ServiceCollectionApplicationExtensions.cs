//using System.Diagnostics.CodeAnalysis;

//using FantasySky.CustomDF;

//namespace Microsoft.Extensions.DependencyInjection;

//public static class ServiceCollectionApplicationExtensions
//{
//    public static IApplicationServiceProvider AddApplication<StartupType>(
//        this IServiceCollection services,
//        Action<ApplicationCreationOptions>? optionsAction = null)
//    {
//        return ApplicationFactory.Create<StartupType>(services, optionsAction);
//    }

//    public static IApplicationServiceProvider AddApplication(
//        this IServiceCollection services,
//        Type startupType,
//        Action<ApplicationCreationOptions>? optionsAction = null)
//    {
//        return ApplicationFactory.Create(startupType, services, optionsAction);
//    }
//}
