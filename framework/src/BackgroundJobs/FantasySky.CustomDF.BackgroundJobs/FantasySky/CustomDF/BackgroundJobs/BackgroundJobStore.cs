using System.Collections.Concurrent;

using FantasySky.CustomDF.DependencyInjection;
using FantasySky.CustomDF.Exceptions;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.BackgroundJobs;

[Dependency(typeof(IBackgroundJobStore), ServiceLifetime.Singleton)]
public class InMemoryBackgroundJobStore : IBackgroundJobStore
{
    private readonly ConcurrentDictionary<Guid, BackgroundJobInfo> _jobs;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryBackgroundJobStore"/> class.
    /// </summary>
    public InMemoryBackgroundJobStore()
    {
        _jobs = new ConcurrentDictionary<Guid, BackgroundJobInfo>();
    }

    public virtual Task<BackgroundJobInfo> FindAsync(Guid jobId)
    {
        return Task.FromResult(_jobs.GetOrDefault(jobId) ?? throw new FrameworkException("The background job can not found."));
    }

    public virtual Task InsertAsync(BackgroundJobInfo jobInfo)
    {
        _jobs[jobInfo.Id] = jobInfo;

        return Task.CompletedTask;
    }

    public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
    {
        var waitingJobs = _jobs.Values
            .Where(t => !t.IsAbandoned && t.NextTryTime <= DateTimeOffset.UtcNow)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount)
            .ToList();

        return Task.FromResult(waitingJobs);
    }

    public virtual Task DeleteAsync(Guid jobId)
    {
        _jobs.TryRemove(jobId, out _);

        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(BackgroundJobInfo jobInfo)
    {
        if (jobInfo.IsAbandoned)
        {
            return this.DeleteAsync(jobInfo.Id);
        }

        return Task.CompletedTask;
    }
}
