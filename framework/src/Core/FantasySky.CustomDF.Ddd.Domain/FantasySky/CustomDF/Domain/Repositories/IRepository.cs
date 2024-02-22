using FantasySky.CustomDF.Domain.Entities;

namespace FantasySky.CustomDF.Domain.Repositories;

/// <summary>
/// Just to mark a class as repository.
/// </summary>
public interface IRepository
{
}

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IBasicRepository<TEntity>
    where TEntity : class, IEntity
{
}

public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>, IBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}
