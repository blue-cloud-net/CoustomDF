using FantasySky.CustomDF.Exceptions;

using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.BackgroundWorkers;

public abstract class BackgroundWorkerBase : IBackgroundWorker, IDisposable
{
    protected readonly ILogger _logger;

    public BackgroundWorkerBase(
        ILogger logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.ServiceProvider = serviceProvider;
        this.StoppingTokenSource = new();
    }

    /// <summary>
    /// 停止令牌
    /// </summary>
    protected CancellationTokenSource StoppingTokenSource { get; }

    /// <summary>
    /// 服务提供者
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Started background worker: " + this.ToString());

        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Stopped background worker: " + this.ToString());
        this.StoppingTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public override string ToString()
    {
        return this.GetType().FullName ?? throw new FrameworkException("Can not get the class name.");
    }

    public void Dispose()
    {
        this.StoppingTokenSource.Dispose();
    }
}
