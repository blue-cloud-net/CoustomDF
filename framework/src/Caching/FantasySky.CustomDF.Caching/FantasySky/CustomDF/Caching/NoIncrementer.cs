using FantasySky.CustomDF.Domain.Values;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FantasySky.CustomDF.Caching;

/// <summary>
/// a distributed no incrementer.
/// </summary>
public class NoIncrementer : DistributedCache<string, CommonNo>, IDistributedCache<string, CommonNo>, INoIncrementer
{
    public NoIncrementer(
        IOptions<DistributedCacheOptions> distributedCacheOption,
        ILogger<NoIncrementer> logger,
        IDistributedCache cache,
        IDistributedCacheSerializer serializer,
        IDistributedCacheKeyNormalizer keyNormalizer) : base(distributedCacheOption, logger, cache, serializer, keyNormalizer)
    {
    }

    public async Task<CommonNo> IncreameAsync(string key, CancellationToken cancellationToken)
    {
        if (this.Cache is ICacheSupportsIncrement incrementCache)
        {
            try
            {
                var result = await incrementCache.IncrementAsync(
                    key,
                    "key",
                    cancellationToken: cancellationToken);

                return new(result);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);

                throw;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<CommonNo> IncrementByDateAsync(string key, CancellationToken cancellationToken)
    {
        if (this.Cache is ICacheSupportsIncrement incrementCache)
        {
            try
            {
                var result = await incrementCache.IncrementAsync(
                    key,
                    DateTimeOffset.UtcNow.ToCustomShortDate(),
                    cancellationToken: cancellationToken);

                return new(result);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);

                throw;
            }
        }

        throw new NotSupportedException();
    }
}
