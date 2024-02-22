namespace FantasySky.CustomDF.Domain.Uow;

public interface IUnitOfWork : IDisposable
{
    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
