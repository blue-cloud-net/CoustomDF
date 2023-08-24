using System.Collections.Concurrent;

using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.DistributedLocking;

[Dependency(typeof(IDistributedLock), ServiceLifetime.Singleton)]
public class LocalDistributedLock : IDistributedLock
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _localSyncObjects = new();

    private readonly IDistributedLockKeyNormalizer _distributedLockKeyNormalizer;

    public LocalDistributedLock(IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        _distributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IDistributedLockHandle?> TryAcquireAsync(string key, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        string normalizeKey = _distributedLockKeyNormalizer.NormalizeKey(key);

        var semaphoreSlim = _localSyncObjects.GetOrAdd(normalizeKey, p => new SemaphoreSlim(1, 1));

        if (await semaphoreSlim.WaitAsync(timeout ?? default, cancellationToken))
        {
            return null;
        }

        return new LocalDistributedLockHandler(semaphoreSlim);
    }
}
