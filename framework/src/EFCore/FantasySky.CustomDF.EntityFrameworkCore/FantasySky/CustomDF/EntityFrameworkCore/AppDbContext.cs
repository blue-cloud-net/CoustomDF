using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.EntityFrameworkCore;

[Dependency(ServiceLifetime.Transient)]
public abstract class AppDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
}
