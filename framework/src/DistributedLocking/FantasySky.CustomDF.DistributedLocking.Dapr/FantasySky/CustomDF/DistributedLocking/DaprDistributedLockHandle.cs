using Dapr.Client;

namespace FantasySky.CustomDF.DistributedLocking;

public class DaprDistributedLockHandle
{
    protected TryLockResponse LockResponse { get; }

    public DaprDistributedLockHandle(TryLockResponse lockResponse)
    {
        this.LockResponse = lockResponse;
    }

    public async ValueTask DisposeAsync()
    {
        await this.LockResponse.DisposeAsync();
    }
}
