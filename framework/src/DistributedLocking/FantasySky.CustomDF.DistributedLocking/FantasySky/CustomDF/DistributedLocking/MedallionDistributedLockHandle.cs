using Medallion.Threading;

namespace FantasySky.CustomDF.DistributedLocking;

internal class MedallionDistributedLockHandle : IDistributedLockHandle
{
    public IDistributedSynchronizationHandle Handle { get; }

    public MedallionDistributedLockHandle(IDistributedSynchronizationHandle handle)
    {
        this.Handle = handle;
    }

    public ValueTask DisposeAsync()
    {
        return this.Handle.DisposeAsync();
    }
}