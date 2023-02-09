using Medallion.Threading;

namespace FantasySky.CustomDF.DistributedLocking;

internal class MedallionDistributedLockHandle : IDistributedLockHandle
{
    public MedallionDistributedLockHandle(IDistributedSynchronizationHandle handle)
    {
        this.Handle = handle;
    }

    public IDistributedSynchronizationHandle Handle { get; }

    public ValueTask DisposeAsync()
    {
        return this.Handle.DisposeAsync();
    }
}
