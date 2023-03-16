using System.Text;

using FantasySky.CustomDF.DependencyInjection;
using FantasySky.CustomDF.Json;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.Caching;

[Dependency(typeof(IDistributedCacheSerializer), ServiceLifetime.Transient)]
public class Utf8JsonDistributedCacheSerializer : IDistributedCacheSerializer
{
    public Utf8JsonDistributedCacheSerializer(
        IJsonSerializer jsonSerializer)
    {
        this.JsonSerializer = jsonSerializer;
    }

    public IJsonSerializer JsonSerializer { get; }

    public T? Deserialize<T>(byte[] bytes)
    {
        return this.JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
    }

    public byte[] Serialize<T>(T obj)
    {
        return Encoding.UTF8.GetBytes(this.JsonSerializer.Serialize(obj));
    }
}
