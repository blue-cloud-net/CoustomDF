using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using FantasySky.CustomDF.Domain.Entities;
using FantasySky.CustomDF.Domain.Entities.Specifications;
using FantasySky.CustomDF.Domain.Guids;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Domain.Repositories;

public abstract class RepositoryBase<TEntity> : IBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// 服务提供器
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    //public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    public IGuidGenerator GuidGenerator { get; }

    public RepositoryBase(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        this.GuidGenerator = _serviceProvider.GetRequiredService<IGuidGenerator>();
    }

    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await this.DeleteAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }

    public abstract Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    public abstract Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    public abstract Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public abstract Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract Task<PagedList<TEntity>> GetPagedListAsync(Page page, bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    public abstract Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await this.InsertAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }

    public abstract Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public virtual async Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await this.UpdateAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return this.GetQueryableAsync();
    }

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return this.GetQueryableAsync();
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        //if (UnitOfWorkManager?.Current != null)
        //{
        //    return UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        //}

        return Task.CompletedTask;
    }
}
