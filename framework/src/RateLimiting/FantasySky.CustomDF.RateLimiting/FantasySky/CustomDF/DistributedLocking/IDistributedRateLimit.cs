using System.Threading.RateLimiting;

namespace FantasySky.CustomDF.DistributedLocking;

public interface IDistributedRateLimit
{
    /// <summary>
    /// Tries to acquire a named lock.
    /// Returns a disposable object to release the lock.
    /// It is suggested to use this method within a using block. 
    /// Returns null if the lock could not be handled.
    /// </summary>
    /// <param name="permitCount">The key name of the lock</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask<RateLimitLease> TryAcquireAsync(int permitCount, CancellationToken cancellationToken = default);
}