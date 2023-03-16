using FantasySky.CustomDF.DependencyInjection;

namespace FantasySky.CustomDF.Domain.Guids;

/// <summary>
/// Implements <see cref="IGuidGenerator"/> by using <see cref="Guid.NewGuid"/>.
/// </summary>
[Dependency(typeof(IGuidGenerator))]
public class SimpleGuidGenerator : IGuidGenerator
{
    public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();

    public virtual Guid Create()
    {
        return Guid.NewGuid();
    }

    public virtual string CreateString(string? format = null)
    {
        return format is null ? this.Create().ToString("N") : this.Create().ToString(format);
    }
}
