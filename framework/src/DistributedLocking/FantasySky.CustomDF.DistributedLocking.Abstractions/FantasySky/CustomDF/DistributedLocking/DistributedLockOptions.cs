namespace FantasySky.CustomDF.DistributedLocking;

public class DistributedLockOptions
{
    /// <summary>
    /// DistributedLock key prefix.
    /// </summary>
    public string KeyPrefix { get; set; }

    public DistributedLockOptions()
    {
        this.KeyPrefix = "";
    }
}
