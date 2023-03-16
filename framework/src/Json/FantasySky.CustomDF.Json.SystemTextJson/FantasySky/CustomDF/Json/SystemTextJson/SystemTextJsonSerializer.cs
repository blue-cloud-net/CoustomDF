using System.Collections.Concurrent;
using System.Text.Json;

using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Json.SystemTextJson;

[Dependency(typeof(IJsonSerializer))]
public class SystemTextJsonSerializer : IJsonSerializer
{

    private static readonly ConcurrentDictionary<JsonSerializerInternalOptions, JsonSerializerOptions> _jsonSerializerOptionsCache = new();

    public SystemTextJsonSerializer(
        IOptionsSnapshot<JsonSerializerOptions> options)
    {
        this.Options = options.Value;
    }

    public JsonSerializerOptions Options { get; }

    public T? Deserialize<T>(string jsonString, bool camelCase = true)
    {
        return JsonSerializer.Deserialize<T>(jsonString, this.CreateJsonSerializerOptions(camelCase));
    }

    public object? Deserialize(Type type, string jsonString, bool camelCase = true)
    {
        return JsonSerializer.Deserialize(jsonString, type, this.CreateJsonSerializerOptions(camelCase));
    }

    public ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, bool camelCase = true)
    {
        return JsonSerializer.DeserializeAsync<T>(utf8Json, this.CreateJsonSerializerOptions(camelCase));
    }

    public string Serialize(object obj, bool camelCase = true, bool indented = false)
    {
        return JsonSerializer.Serialize(obj, this.CreateJsonSerializerOptions(camelCase, indented));
    }

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
    {
        var options = _jsonSerializerOptionsCache.GetOrAdd(new(camelCase, indented, this.Options), _ =>
        {
            var settings = new JsonSerializerOptions(this.Options);

            if (camelCase)
            {
                settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }

            if (indented)
            {
                settings.WriteIndented = true;
            }

            return settings;
        });

        return options;
    }
}

public record JsonSerializerInternalOptions(bool CamelCase, bool Indented, JsonSerializerOptions Options);
