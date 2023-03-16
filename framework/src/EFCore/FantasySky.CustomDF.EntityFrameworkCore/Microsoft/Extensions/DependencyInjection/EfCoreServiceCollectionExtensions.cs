using FantasySky.CustomDF.EntityFrameworkCore;
using FantasySky.CustomDF.Modularity;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class EfCoreServiceCollectionExtensions
{
    public static void AddDbContext<TDbContext>(
        this ServiceConfigurationContext context,
        Action<DbContextOptionsBuilder> optionsBuilder)
        where TDbContext : DbContext
    {
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(DbContextProvider<>));

        context.Services.AddPooledDbContextFactory<TDbContext>(optionsBuilder);

        return;
    }
}
