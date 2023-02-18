using FantasySky.CustomDF.DependencyInjection;

namespace FantasySky.CustomDF.DistributedLocking;

[Dependency(typeof(IDistributedLock), ReplaceServices = true)]
public class DaprDistributedLock : IDistributedLock
{
    public Task<IDistributedLockHandle?> TryAcquireAsync(
        string key, TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
