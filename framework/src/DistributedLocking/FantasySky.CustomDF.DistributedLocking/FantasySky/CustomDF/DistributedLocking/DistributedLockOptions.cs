namespace FantasySky.CustomDF.DistributedLocking;

public class DistributedLockOptions
{
    public DistributedLockOptions()
    {
        this.KeyPrefix = String.Empty;
    }

    /// <summary>
    /// Cache key prefix.
    /// </summary>
    public string KeyPrefix { get; set; }
}
