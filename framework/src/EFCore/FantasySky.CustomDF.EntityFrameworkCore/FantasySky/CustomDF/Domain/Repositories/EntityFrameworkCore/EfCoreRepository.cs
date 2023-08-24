using System.Linq.Expressions;

using FantasySky.CustomDF.Domain.Entities.Specifications;
using FantasySky.CustomDF.EntityFrameworkCore;
using FantasySky.CustomDF.Exceptions;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Domain.Repositories.EntityFrameworkCore;

public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    public EfCoreRepository(
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        this.DbContextProvider = serviceProvider.GetRequiredService<IDbContextProvider<TDbContext>>();
    }

    public IDbContextProvider<TDbContext> DbContextProvider { get; }

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        this.CheckAndSetId(entity);

        var dbContext = await this.GetDbContextAsync();

        var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return savedEntity;
    }

    public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();

        foreach (var entity in entityArray)
        {
            this.CheckAndSetId(entity);
        }

        var dbContext = await this.GetDbContextAsync();

        await dbContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task<bool> DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();

        dbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
        {
            var deleteCount = await dbContext.SaveChangesAsync(cancellationToken);

            return deleteCount > 0;
        }

        return true;
    }

    public override async Task<int> DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();

        dbContext.RemoveRange(entities);

        if (autoSave)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        return 0;
    }

    public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);

        await this.DeleteManyAsync(entities, autoSave, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        await dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();

        dbContext.Attach(entity);

        var updatedEntity = dbContext.Update(entity).Entity;

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return updatedEntity;
    }

    public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await this.GetDbContextAsync();

        dbContext.Set<TEntity>().UpdateRange(entities);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public override async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        => await (await this.GetDbSetAsync()).AnyAsync(cancellationToken);

    public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await (await this.GetDbSetAsync()).Where(predicate).AnyAsync(cancellationToken);

    public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => await (await this.GetDbSetAsync()).LongCountAsync(cancellationToken);

    public override async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await (await this.GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);

    public override async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
    => includeDetails
            ? await (await this.WithDetailsAsync()).FirstOrDefaultAsync(predicate, cancellationToken)
            : await (await this.GetDbSetAsync()).FirstOrDefaultAsync(predicate, cancellationToken);

    public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        => includeDetails
            ? await (await this.WithDetailsAsync()).AsNoTracking().ToListAsync(cancellationToken)
            : await (await this.GetDbSetAsync()).AsNoTracking().ToListAsync(cancellationToken);

    public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        => includeDetails
            ? await (await this.WithDetailsAsync()).AsNoTracking().Where(predicate).ToListAsync(cancellationToken)
            : await (await this.GetDbSetAsync()).AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public override async Task<PagedList<TEntity>> GetPagedListAsync(Page page, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails
            ? await this.WithDetailsAsync()
            : await this.GetDbSetAsync();

        return await queryable.AsNoTracking().ToPageListAsync(page, cancellationToken);
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
        => (await this.GetDbSetAsync()).AsQueryable();

    protected override async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await (await this.GetDbContextAsync()).SaveChangesAsync(cancellationToken);

    async Task<DbContext> IEfCoreRepository<TEntity>.GetDbContextAsync()
    {
        return await this.GetDbContextAsync() as DbContext ?? throw new InvalidCastException($"Convert to type '{nameof(DbContext)}' failed.");
    }

    Task<DbSet<TEntity>> IEfCoreRepository<TEntity>.GetDbSetAsync()
    {
        return this.GetDbSetAsync();
    }

    protected virtual Task<TDbContext> GetDbContextAsync()
    {
        // Multi-tenancy unaware entities should always use the host connection string
        //if (!EntityHelper.IsMultiTenant<TEntity>())
        //{
        //    using (CurrentTenant.Change(null))
        //    {
        //        return _dbContextProvider.GetDbContextAsync();
        //    }
        //}

        // TODO
        return this.DbContextProvider.GetDbContextAsync();
    }

    protected async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        return (await this.GetDbContextAsync()).Set<TEntity>();
    }

    protected virtual void CheckAndSetId(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId)
        {
            this.TrySetGuidId(entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => this.GuidGenerator.Create()
        );
    }
}

public class EfCoreRepository<TDbContext, TEntity, TKey> :
    EfCoreRepository<TDbContext, TEntity>,
    IEfCoreRepository<TEntity, TKey>

    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    public EfCoreRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task<bool> DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await this.FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return false;
        }

        return await this.DeleteAsync(entity, autoSave, cancellationToken);
    }

    public async Task<int> DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entities = await (await this.GetDbSetAsync()).Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        return await this.DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    public async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await this.WithDetailsAsync()).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken)
            : await (await this.GetDbSetAsync()).FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<TEntity?> FindAsNoTrackingAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await this.WithDetailsAsync()).OrderBy(e => e.Id).AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken)
            : await (await this.GetDbSetAsync()).AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        => await this.FindAsync(id, includeDetails, cancellationToken) ?? throw new EntityNotFoundException(typeof(TEntity), id);

    public async Task<TEntity> GetAsNoTrackingAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        => await this.FindAsNoTrackingAsync(id, includeDetails, cancellationToken) ?? throw new EntityNotFoundException(typeof(TEntity), id);
}
