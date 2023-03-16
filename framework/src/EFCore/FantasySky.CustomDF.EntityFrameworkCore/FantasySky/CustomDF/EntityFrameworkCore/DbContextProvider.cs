namespace FantasySky.CustomDF.EntityFrameworkCore;

public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public DbContextProvider(
        IDbContextFactory<TDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        return _dbContextFactory.CreateDbContextAsync(cancellationToken);
    }
}
