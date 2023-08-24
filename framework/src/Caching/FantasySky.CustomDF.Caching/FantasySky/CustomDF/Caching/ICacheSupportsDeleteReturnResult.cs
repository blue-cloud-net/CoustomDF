namespace FantasySky.CustomDF.Caching;

/// <summary>
/// 分布式缓存支持删除返回结果
/// </summary>
public interface ICacheSupportsDeleteReturnResult
{
    /// <summary>
    /// 移除key，并返回结果
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool RemoveWithResult(string key);

    /// <summary>
    /// 移除key，并返回结果
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RemoveWithResultAsync(string key, CancellationToken cancellationToken = default);
}
