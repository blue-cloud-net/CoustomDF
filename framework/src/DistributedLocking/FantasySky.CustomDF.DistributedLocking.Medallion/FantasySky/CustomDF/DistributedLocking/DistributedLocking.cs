using FantasySky.CustomDF.DependencyInjection;

using Medallion.Threading;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DistributedLocking;

[Dependency(typeof(IDistributedLock), ServiceLifetime.Transient, ReplaceServices = true)]
public class DistributedLock : IDistributedLock
{
    public DistributedLock(
        IDistributedLockKeyNormalizer distributedLockKeyNormalizer,
        IDistributedLockProvider distributedLockProvider)
    {
        this.DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
        this.DistributedLockProvider = distributedLockProvider;
    }

    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    protected IDistributedLockProvider DistributedLockProvider { get; }

    public async Task<IDistributedLockHandle?> TryAcquireAsync(
        string key, TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var normalizeKey = this.DistributedLockKeyNormalizer.NormalizeKey(key);

        var handler = await this.DistributedLockProvider
            .TryAcquireLockAsync(normalizeKey, timeout ?? default, cancellationToken);

        return handler is null ? null : new DistributedLockHandle(handler);
    }
}
