using System.Security.Cryptography;
using System.Text;

namespace System;

public static class BytesExtensions
{
    public static string ToMd5(this byte[] inputBytes)
    {
        var hashBytes = MD5.HashData(inputBytes);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            sb.Append(hashByte.ToString("X2"));
        }

        return sb.ToString();
    }

    public static string ToSha256(this byte[] inputBytes)
    {
        var hashBytes = SHA256.HashData(inputBytes);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes)
        {
            sb.Append(hashByte.ToString("X2"));
        }

        return sb.ToString();
    }
}
