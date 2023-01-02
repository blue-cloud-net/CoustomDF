namespace FantasySky.CustomDF.Caching;

public class DistributedCacheOptions
{
    public DistributedCacheOptions()
    {
        this.KeyPrefix = String.Empty;
    }

    /// <summary>
    /// Cache key prefix.
    /// </summary>
    public string KeyPrefix { get; set; }
}
