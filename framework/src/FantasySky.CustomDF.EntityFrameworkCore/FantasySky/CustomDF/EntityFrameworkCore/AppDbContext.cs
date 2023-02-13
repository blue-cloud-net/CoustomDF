using FantasySky.CustomDF.DependencyInjection;

namespace FantasySky.CustomDF.EntityFrameworkCore;

public abstract class AppDbContext<TDbContext> : DbContext, ITransientDependency
    where TDbContext : DbContext
{
}
