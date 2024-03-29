using System.Security.Cryptography;
using System.Text;

namespace System.IO;

/// <summary>
/// Extension methods for <see cref="Stream"/>.
/// </summary>
public static class StreamExtensions
{
    public static Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        return stream.CopyToAsync(
            destination,
            81920, //this is already the default value, but needed to set to be able to pass the cancellationToken
            cancellationToken
        );
    }

    public static byte[] GetAllBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }

    public static string ToMd5(this Stream stream)
    {
        var hashBytes = MD5.HashData(stream);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            sb.Append(hashByte.ToString("X2"));
        }

        return sb.ToString();
    }

    public static string ToSha256(this Stream stream)
    {
        var hashBytes = SHA256.HashData(stream);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            sb.Append(hashByte.ToString("X2"));
        }

        return sb.ToString();
    }
}
