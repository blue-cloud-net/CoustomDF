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