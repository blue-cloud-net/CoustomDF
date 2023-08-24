using FantasySky.CustomDF.Exceptions;

namespace FantasySky.CustomDF.Domain.Repositories.EntityFrameworkCore;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<DbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();
}

public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
}
