using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DependencyInjection;

public interface IConventionalRegistrar
{
    void AddAssembly(IServiceCollection services, Assembly assembly);

    void AddType(IServiceCollection services, Type type);

    void AddTypes(IServiceCollection services, params Type[] types);
}
