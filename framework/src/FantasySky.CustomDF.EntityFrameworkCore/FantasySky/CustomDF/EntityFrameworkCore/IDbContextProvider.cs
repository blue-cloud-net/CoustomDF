namespace FantasySky.CustomDF.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEFCoreDbContext
{
    Task<TDbContext> GetDbContextAsync();
}
