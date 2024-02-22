using System.Security.Cryptography;

namespace System;

/// <summary>
/// <see cref="Byte[]"/>数组扩展方法
/// </summary>
public static class BytesExtensions
{
    /// <summary>
    /// MD5Hash，返回16进制字符串
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToMd5(this byte[] inputBytes)
    {
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// SHA1Hash，返回16进制字符串
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    [Obsolete("Hash长度太低，容易造成Hash冲突，尽量不要使用")]
    public static string ToSha1(this byte[] inputBytes)
    {
        var hashBytes = SHA1.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// SHA256Hash，返回16进制字符串
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToSha256(this byte[] inputBytes)
    {
        var hashBytes = SHA256.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// SHA384Hash，返回16进制字符串
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToSha384(this byte[] inputBytes)
    {
        var hashBytes = SHA384.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// SHA512Hash，返回16进制字符串
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToSha512(this byte[] inputBytes)
    {
        var hashBytes = SHA512.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// Base64编码
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToBase64String(this byte[] inputBytes)
        => Convert.ToBase64String(inputBytes);

    /// <summary>
    /// Hex编码
    /// </summary>
    /// <param name="inputBytes"></param>
    /// <returns></returns>
    public static string ToHexString(this byte[] inputBytes)
        => Convert.ToHexString(inputBytes);
}