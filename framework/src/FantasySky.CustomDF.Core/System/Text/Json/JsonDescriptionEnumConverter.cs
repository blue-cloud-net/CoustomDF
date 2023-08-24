using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Converter to convert enums to and from <see cref="DescriptionAttribute" />.
/// </summary>
/// <remarks>
/// Reading is case insensitive, writing can be customized via a <see cref="JsonNamingPolicy" />.
/// </remarks>
public class JsonDescriptionEnumConverter : JsonConverterFactory
{
    /// <summary>
    /// Constructor. Creates the <see cref="JsonDescriptionEnumConverter"/> with the
    /// default naming policy and allows integer values.
    /// </summary>
    public JsonDescriptionEnumConverter()
    {
        // An empty constructor is needed for construction via attributes
    }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = (JsonConverter)Activator.CreateInstance(
            typeof(JsonDescriptionEnumConverter<>).MakeGenericType(typeToConvert),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            new object?[] { },
            culture: null)!;

        return converter;
    }
}

public class JsonDescriptionEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private FieldInfo[] _enumFileds;

    public JsonDescriptionEnumConverter()
    {
        _enumFileds = typeof(T).GetFields();
    }

    public override bool CanConvert(Type type)
    {
        return type.IsEnum;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
        {
            throw new JsonException();
        }

        var desc = reader.GetString();

        if (desc.IsNullOrWhiteSpace())
        {
            throw new JsonException();
        }

        // 先尝试用Description转，找不到再Name转
        var filed = _enumFileds.FirstOrDefault(p => p.GetCustomAttribute<DescriptionAttribute>()?.Description == desc);
        if (filed is not null)
        {
            return (T)filed.GetValue(null)!;
        }

        var result = Enum.Parse<T>(desc);

        return result;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var filed = _enumFileds.FirstOrDefault(p => p.Name == value.ToString())
            ?? throw new JsonException("The enum value can not found.");

        var descAttribute = filed.GetCustomAttribute<DescriptionAttribute>();

        var result = descAttribute?.Description;

        if (result.IsNullOrWhiteSpace())
        {
            // Description为空的情况，使用Name
            result = Enum.GetName(value);
        }

        if (result.IsNullOrWhiteSpace())
        {
            throw new JsonException($"Unexpected '{typeof(T).Name}' enum value '{value}'");
        }

        writer.WriteStringValue(descAttribute?.Description ?? value.GetName() ?? value.ToString());
    }
}
