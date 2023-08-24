using FantasySky.CustomDF.BackgroundWorkers;
using FantasySky.CustomDF.DistributedLocking;
using FantasySky.CustomDF.Threading;

using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.BackgroundJobs;

public abstract class BackgroundJobWorker : AsyncPeriodicBackgroundWorkerBase, IBackgroundJobWorker
{
    protected const string DistributedLockName = "BackgroundJobWorker";

    protected IDistributedLock DistributedLock { get; }

    public BackgroundJobWorker(
        ILogger<BackgroundJobWorker> logger,
        AsyncPeriodicTimer timer,
        IServiceProvider serviceProvider,
        IDistributedLock distributedLock) : base(logger, timer, serviceProvider)
    {
        this.DistributedLock = distributedLock;
    }

    public abstract string JobName { get; }

    protected abstract ValueTask DoWorkCoreAsync(PeriodicBackgroundWorkerContext workerContext);

    protected override async ValueTask DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using var handler = await this.DistributedLock
            .TryAcquireAsync($"{DistributedLockName}:{this.JobName}", null, base.StoppingTokenSource.Token);

        if (handler != null)
        {
            await this.DoWorkCoreAsync(workerContext);
        }
    }
}
