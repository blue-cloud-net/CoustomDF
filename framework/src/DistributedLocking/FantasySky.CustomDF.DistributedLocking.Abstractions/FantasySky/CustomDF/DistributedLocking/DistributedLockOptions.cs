namespace FantasySky.CustomDF.DistributedLocking;

public class DistributedLockOptions
{
    public static string Path = "DistributedLock";

    public DistributedLockOptions()
    {
        this.KeyPrefix = "";
    }

    /// <summary>
    /// DistributedLock key prefix.
    /// </summary>
    public string KeyPrefix { get; set; }

    public string ProviderName { get; set; } = String.Empty;

    public string Configuration { get; set; } = String.Empty;
}
