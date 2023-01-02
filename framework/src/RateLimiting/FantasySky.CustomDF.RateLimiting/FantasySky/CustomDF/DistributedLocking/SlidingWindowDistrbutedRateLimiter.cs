using System.Threading.RateLimiting;

using FantasySky.CustomDF.Caching;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.DistributedLocking;

public class SlidingWindowDistrbutedRateLimiter : ReplenishingRateLimiter
{
    private readonly SlidingWindowRateLimiterOptions _options;
    private readonly IDistributedCache<string> _cache;

    public override TimeSpan ReplenishmentPeriod { get; }

    public override bool IsAutoReplenishing { get; }

    public override TimeSpan? IdleDuration => throw new NotImplementedException();

    public string Name { get; }

    public SlidingWindowDistrbutedRateLimiter(
        SlidingWindowRateLimiterOptions options,
        IDistributedCache<string> cache,
        ILogger logger,
        string name)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        else if (options.PermitLimit <= 0)
        {
            throw new ArgumentException(SR.Format(SR.ShouldBeGreaterThan0, nameof(options.PermitLimit)), nameof(options));
        }
        else if (options.SegmentsPerWindow <= 0)
        {
            throw new ArgumentException(SR.Format(SR.ShouldBeGreaterThan0, nameof(options.SegmentsPerWindow)), nameof(options));
        }
        else if (options.QueueLimit < 0)
        {
            throw new ArgumentException(SR.Format(SR.ShouldBeGreaterThanOrEqual0, nameof(options.QueueLimit)), nameof(options));
        }
        else if (options.Window <= TimeSpan.Zero)
        {
            throw new ArgumentException(SR.Format(SR.ShouldBeGreaterThanTimeSpan0, nameof(options.Window)), nameof(options));
        }

        _options = new SlidingWindowRateLimiterOptions
        {
            PermitLimit = options.PermitLimit,
            QueueProcessingOrder = options.QueueProcessingOrder,
            QueueLimit = options.QueueLimit,
            Window = options.Window,
            SegmentsPerWindow = options.SegmentsPerWindow,
            AutoReplenishment = options.AutoReplenishment
        };

        _cache = cache;
        this.Name = name;
    }

    public override RateLimiterStatistics? GetStatistics()
    {
        throw new NotImplementedException();
    }

    public override bool TryReplenish()
    {
        throw new NotImplementedException();
    }

    protected override ValueTask<RateLimitLease> AcquireAsyncCore(int permitCount, CancellationToken cancellationToken)
    {
        var acquireTime = DateTimeOffset.UtcNow;

        var cacheEntryOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = _options.Window,
        };

        _cache.SortedSetAddAsync("RateLimit:" + this.Name, acquireTime.Ticks, "", cacheEntryOptions, cancellationToken);
    }

    protected override RateLimitLease AttemptAcquireCore(int permitCount)
    {
        throw new NotImplementedException();
    }
}
