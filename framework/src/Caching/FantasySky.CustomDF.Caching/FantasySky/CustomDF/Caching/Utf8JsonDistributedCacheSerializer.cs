using System.Text;
using System.Text.Json;

using FantasySky.CustomDF.DependencyInjection;
using FantasySky.CustomDF.Serialize.Json;

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
        Check.NotNull(obj, nameof(obj));

        return Encoding.UTF8.GetBytes(this.JsonSerializer.Serialize(obj));
    }
}
