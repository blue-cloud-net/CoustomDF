using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.DistributedLocking;

[Dependency(typeof(IDistributedLockKeyNormalizer), ServiceLifetime.Transient)]
public class DistributedLockKeyNormalizer : IDistributedLockKeyNormalizer
{
    protected DistributedLockOptions Options { get; }

    public DistributedLockKeyNormalizer(IOptions<DistributedLockOptions> options)
    {
        this.Options = options.Value;
    }

    public virtual string NormalizeKey(string name)
    {
        return $"{this.Options.KeyPrefix}{name}";
    }
}
