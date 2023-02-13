using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using FantasySky.CustomDF.Domain.Entities.Specifications;
using FantasySky.CustomDF.EntityFrameworkCore;

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

    public override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        => await (await this.GetDbSetAsync()).LongCountAsync(cancellationToken);

    public override Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        => includeDetails
            ? await (await this.WithDetailsAsync()).ToListAsync(cancellationToken)
            : await (await this.GetDbSetAsync()).ToListAsync(cancellationToken);

    public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        => includeDetails
            ? await (await this.WithDetailsAsync()).Where(predicate).ToListAsync(cancellationToken)
            : await (await this.GetDbSetAsync()).Where(predicate).ToListAsync(cancellationToken);

    public override async Task<PagedList<TEntity>> GetPagedListAsync(Page page, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails
            ? await this.WithDetailsAsync()
            : await this.GetDbSetAsync();

        return await queryable.ToPageListAsync(page, cancellationToken);
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
        => (await this.GetDbSetAsync()).AsQueryable();

    public override async Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
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

    public override async Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
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
