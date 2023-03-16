namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

public class RedisCacheOptions
{
    public static string Path = "DistributedCache:Redis";

    /// <summary>
    /// 是否启用Redis分布式缓存
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The configuration used to connect to Redis.
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// The Redis instance name.
    /// </summary>
    public string? InstanceName { get; set; }
}
