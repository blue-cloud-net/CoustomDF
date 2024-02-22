namespace FantasySky.CustomDF.Serialize;

/// <summary>
/// 文本序列化器接口
/// </summary>
public interface ITextSerializer : ISerializer
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="camelCase"></param>
    /// <param name="indented"></param>
    /// <returns></returns>
    string Serialize(object obj, bool camelCase = true, bool indented = false);

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="camelCase"></param>
    /// <returns></returns>
    T? Deserialize<T>(string text, bool camelCase = true);

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="camelCase"></param>
    /// <returns></returns>
    T? Deserialize<T>(ReadOnlySpan<char> text, bool camelCase = true);

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="utf8Text"></param>
    /// <param name="camelCase"></param>
    /// <returns></returns>
    ValueTask<T?> DeserializeAsync<T>(Stream utf8Text, bool camelCase = true);

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="type"></param>
    /// <param name="text"></param>
    /// <param name="camelCase"></param>
    /// <returns></returns>
    object? Deserialize(Type type, string text, bool camelCase = true);
}
