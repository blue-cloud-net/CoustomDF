namespace FantasySky.CustomDF.DistributedLocking;

public class LocalDistributedLockHandler : IDistributedLockHandle
{
    private readonly SemaphoreSlim _semaphore;

    public LocalDistributedLockHandler(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
    }

    public ValueTask DisposeAsync()
    {
        _semaphore.Release();
        return default;
    }
}
