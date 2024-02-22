namespace FantasySky.CustomDF.Storage;

/// <summary>
/// 存储服务
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// 从存储中获取文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取文件下载地址
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Uri?> GetDownloadUriAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAsync(
        string path,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default);
}