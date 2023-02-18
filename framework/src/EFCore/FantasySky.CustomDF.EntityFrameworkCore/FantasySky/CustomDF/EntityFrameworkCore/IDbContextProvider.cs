namespace FantasySky.CustomDF.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync();
}
