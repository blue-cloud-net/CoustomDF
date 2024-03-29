namespace FantasySky.CustomDF.Json;

public interface IJsonSerializer
{
    string Serialize(object obj, bool camelCase = true, bool indented = false);

    T? Deserialize<T>(string jsonString, bool camelCase = true);

    ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, bool camelCase = true);

    object? Deserialize(Type type, string jsonString, bool camelCase = true);
}
