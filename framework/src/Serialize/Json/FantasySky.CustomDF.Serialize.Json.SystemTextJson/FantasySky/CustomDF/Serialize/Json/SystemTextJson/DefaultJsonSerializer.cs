using System.Text.Json;

namespace FantasySky.CustomDF.Serialize.Json.SystemTextJson;

/// <summary>
/// 默认的Json序列化器
/// </summary>
public static class DefaultJsonSerializer
{
    /// <summary>
    /// 实例
    /// </summary>
    public static IJsonSerializer Instance = new SystemTextJsonSerializer(new JsonSerializerOptions());
}
