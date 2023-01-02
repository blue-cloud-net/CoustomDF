namespace FantasySky.CustomDF.DistributedLocking;

public interface IDistributedLock
{
    /// <summary>
    /// Tries to acquire a named lock.
    /// Returns a disposable object to release the lock.
    /// It is suggested to use this method within a using block. 
    /// Returns null if the lock could not be handled.
    /// </summary>
    /// <param name="key">The key name of the lock</param>
    /// <param name="timeout">How long to wait before giving up on the acquisition attempt. Defaults to 0</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IDistributedLockHandle> TryAcquireAsync(string key, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
}