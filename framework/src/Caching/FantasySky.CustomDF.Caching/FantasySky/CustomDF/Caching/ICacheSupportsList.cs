namespace FantasySky.CustomDF.Caching;

public interface ICacheSupportsList
{
    /// <summary>
    /// 移出并获取列表的第一个元素
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<byte[]?> ListLPopAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将值插入到列表头部
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ListLPushAsync(string key, byte[] value, CancellationToken cancellationToken = default);

    /// <summary>
    /// 移出并获取列表的最后一个元素
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<byte[]?> ListRPopAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///  将值插入到列表尾部
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ListRPushAsync(string key, byte[] value, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表指定范围内的元素
    /// </summary>
    /// <param name="key"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<byte[][]> ListRangeAsync(string key, long? min = null, long? max = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表长度
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> ListCountAsync(string key, CancellationToken cancellationToken = default);
}
