using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FantasySky.CustomDF.EntityFrameworkCore.ValueConverters;

public class JsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
{
    private static readonly JsonSerializerOptions _deserializeOptions = new()
    {
        Converters =
        {
            // TODO 还没看完，没理解意义
            //new ObjectToInferredTypesConverter()
        }
    };

    public JsonValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s) ?? default!)
    {
    }

    public static JsonValueConverter<TPropertyType> Instance = new();

    private static string SerializeObject(TPropertyType d)
    {
        return JsonSerializer.Serialize(d);
    }

    private static TPropertyType? DeserializeObject(string s)
    {
        return JsonSerializer.Deserialize<TPropertyType>(s, _deserializeOptions);
    }
}
