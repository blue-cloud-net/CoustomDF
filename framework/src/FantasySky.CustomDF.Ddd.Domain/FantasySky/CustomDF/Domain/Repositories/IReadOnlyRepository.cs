using System.Linq.Expressions;

using FantasySky.CustomDF.Domain.Entities;
using FantasySky.CustomDF.Exceptions;

namespace FantasySky.CustomDF.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IRepository
    where TEntity : class, IEntity
{
    //IAsyncQueryableExecuter AsyncExecuter { get; }

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements by the given <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets total count of all entities.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets total count of all entities.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity or null</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all the entities.
    /// </summary>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of entities by the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="includeDetails">Set true to include details (sub-collections) of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IRepository
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity or null</returns>
    Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity with given primary key.
    /// Throws <see cref="EntityNotFoundException"/> if can not find an entity with given id.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);
}
