namespace FantasySky.CustomDF.DistributedLocking;

public class MedallionDistributedLock : IDistributedLock
{
    public Task<IDistributedLockHandle> TryAcquireAsync(string key, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
