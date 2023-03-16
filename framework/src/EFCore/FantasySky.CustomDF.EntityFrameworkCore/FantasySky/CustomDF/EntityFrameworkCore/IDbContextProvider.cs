namespace FantasySky.CustomDF.EntityFrameworkCore;

/// <summary>
/// DbContext提供器
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public interface IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// 获取DbContext
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}
