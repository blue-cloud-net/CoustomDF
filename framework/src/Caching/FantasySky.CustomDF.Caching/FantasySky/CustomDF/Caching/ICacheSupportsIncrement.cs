namespace FantasySky.CustomDF.Caching;

/// <summary>
/// 缓存支持自增
/// </summary>
public interface ICacheSupportsIncrement
{
    /// <summary>
    /// 自增
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="filed">字段</param>
    /// <param name="increment">每次自增量</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> IncrementAsync(string key, string filed, long increment = 1, CancellationToken cancellationToken = default);
}
