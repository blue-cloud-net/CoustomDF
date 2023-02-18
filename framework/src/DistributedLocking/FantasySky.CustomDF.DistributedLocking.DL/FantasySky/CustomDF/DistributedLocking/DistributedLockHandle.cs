using Medallion.Threading;

namespace FantasySky.CustomDF.DistributedLocking;

internal class DistributedLockHandle : IDistributedLockHandle
{
    public DistributedLockHandle(IDistributedSynchronizationHandle handle)
    {
        this.Handle = handle;
    }

    protected IDistributedSynchronizationHandle Handle { get; }

    public ValueTask DisposeAsync()
    {
        return this.Handle.DisposeAsync();
    }
}
