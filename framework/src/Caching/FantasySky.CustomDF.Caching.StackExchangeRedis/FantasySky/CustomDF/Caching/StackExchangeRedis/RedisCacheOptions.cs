namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

public class RedisCacheOptions
{
    /// <summary>
    /// The configuration used to connect to Redis.
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// The Redis instance name.
    /// </summary>
    public string? InstanceName { get; set; }
}
