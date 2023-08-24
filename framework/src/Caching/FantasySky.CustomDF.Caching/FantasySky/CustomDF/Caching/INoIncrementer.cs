using FantasySky.CustomDF.Domain.Values;

namespace FantasySky.CustomDF.Caching;

/// <summary>
/// 分布式序号自增器
/// </summary>
public interface INoIncrementer
{
    /// <summary>
    /// 根据Key自增，可附带field的第二Key
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The cache item, or null.</returns>
    Task<CommonNo> IncreametAsync(
        string key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据Key自增，可附带field的第二Key
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="field"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The cache item, or null.</returns>
    Task<CommonNo> IncreametAsync(
        string key,
        string field,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据Key自增，可附带field的第二Key
    /// 根据自定义的构建方法构建返回值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="buildNoAction"></param>
    /// <param name="field"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<CommonNo> IncreametAsync(
        string key,
        string? field = null,
        Func<CommonNo, string>? buildNoAction = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据Key和日期自增
    /// </summary>
    /// <param name="key"></param>
    /// <param name="buildNoAction">如不填则使用默认规则生成</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CommonNo> IncrementByDateAsync(
        string key,
        Func<CommonNo, string>? buildNoAction = null,
        CancellationToken cancellationToken = default
    );
}
