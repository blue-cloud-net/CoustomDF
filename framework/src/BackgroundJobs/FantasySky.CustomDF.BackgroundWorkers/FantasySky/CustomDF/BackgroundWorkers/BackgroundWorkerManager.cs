using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.BackgroundWorkers;

[Dependency(typeof(IBackgroundWorkerManager), ServiceLifetime.Singleton)]
public class BackgroundWorkerManager : IBackgroundWorkerManager, IDisposable
{
    private readonly List<IBackgroundWorker> _backgroundWorkers;

    public BackgroundWorkerManager()
    {
        _backgroundWorkers = new();
    }

    protected bool IsRunning { get; private set; }

    public async Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        _backgroundWorkers.Add(worker);

        if (this.IsRunning)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        this.IsRunning = true;

        foreach (var worker in _backgroundWorkers)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        this.IsRunning = false;

        foreach (var worker in _backgroundWorkers)
        {
            await worker.StopAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
