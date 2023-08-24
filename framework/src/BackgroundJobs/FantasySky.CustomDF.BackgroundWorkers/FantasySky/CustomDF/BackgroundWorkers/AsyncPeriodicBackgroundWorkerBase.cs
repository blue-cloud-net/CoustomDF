using FantasySky.CustomDF.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.BackgroundWorkers;

public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
{
    public AsyncPeriodicBackgroundWorkerBase(
        ILogger logger,
        AsyncPeriodicTimer timer,
        IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
        this.Timer = timer;
    }

    public AsyncPeriodicTimer Timer { get; }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        this.Timer.DoWork += this.Timer_DoWork;

        await base.StartAsync(cancellationToken);
        await this.Timer.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await this.Timer.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async ValueTask Timer_DoWork(AsyncPeriodicTimer timer, CancellationToken cancellationToken)
    {
        _ = timer;
        await this.DoWorkAsync(cancellationToken);
    }

    private async ValueTask DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using var scope = this.ServiceProvider.CreateAsyncScope();
        try
        {
            await this.DoWorkAsync(new PeriodicBackgroundWorkerContext(scope.ServiceProvider, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"The background worker '${base.ToString()}' throw a no handler exception.");
        }
    }

    protected abstract ValueTask DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
}
