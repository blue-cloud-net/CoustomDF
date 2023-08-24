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

    public Task<CommonNo> IncreametAsync(string key, CancellationToken cancellationToken = default)
        => this.IncreametAsync(key, cancellationToken: cancellationToken);

    public Task<CommonNo> IncreametAsync(string key, string field, CancellationToken cancellationToken = default)
        => this.IncreametAsync(key, field, cancellationToken: cancellationToken);

    public async Task<CommonNo> IncreametAsync(
        string key,
        string? field = null,
        Func<CommonNo, string>? buildNoAction = null,
        CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsIncrement incrementCache)
        {
            try
            {
                var result = await incrementCache.IncrementAsync(
                    key,
                    field.IsNullOrWhiteSpace() ? "key" : field,
                    cancellationToken: cancellationToken);

                return buildNoAction is null ? new(result) : new(result, buildNoAction);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex);

                throw;
            }
        }

        throw new NotSupportedException();
    }

    public async Task<CommonNo> IncrementByDateAsync(
        string key,
        Func<CommonNo, string>? buildNoAction = null,
        CancellationToken cancellationToken = default)
    {
        if (this.Cache is ICacheSupportsIncrement incrementCache)
        {
            try
            {
                var result = await incrementCache.IncrementAsync(
                    key,
                    DateTimeOffset.UtcNow.ToCustomShortDate(),
                    cancellationToken: cancellationToken);

                return buildNoAction is null ? new(result) : new(result, buildNoAction);
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
