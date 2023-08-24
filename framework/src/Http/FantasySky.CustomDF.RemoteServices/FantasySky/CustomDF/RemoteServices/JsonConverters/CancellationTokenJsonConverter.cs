using System.Text.Json;
using System.Text.Json.Serialization;

namespace FantasySky.CustomDF.RemoteServices.JsonConverters;

/// <summary>
/// 使用转换器排除掉CancellationToken的转换
/// </summary>
public class CancellationTokenJsonConverter : JsonConverter<CancellationToken>
{
    public override CancellationToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default;
    }

    public override void Write(Utf8JsonWriter writer, CancellationToken value, JsonSerializerOptions options)
    {
        return;
    }
}
