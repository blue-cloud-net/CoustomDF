namespace FantasySky.CustomDF.Storage;

/// <summary>
/// 文件系统存储服务
/// </summary>
public class FileSystemStorageService : IStorageService
{
    public Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Uri?> GetDownloadUriAsync(string path, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(string path, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}