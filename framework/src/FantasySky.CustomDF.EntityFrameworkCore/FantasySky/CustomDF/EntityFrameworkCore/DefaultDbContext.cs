using FantasySky.CustomDF.DependencyInjection;

namespace FantasySky.CustomDF.EntityFrameworkCore;

public abstract class DefaultDbContext<TDbContext> : DbContext, IEFCoreDbContext, ITransientDependency
    where TDbContext : DbContext
{

}