using FantasySky.CustomDF.DependencyInjection;
using FantasySky.CustomDF.Exceptions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.Threading;

[Dependency(ServiceLifetime.Transient)]
public class AsyncPeriodicTimer : IDisposable
{
    private Timer? _timer;

    private readonly ILogger<AsyncPeriodicTimer> _logger;
    private readonly SemaphoreSlim _interlock;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AsyncPeriodicTimer(
        ILogger<AsyncPeriodicTimer> logger)
    {
        _logger = logger;
        _interlock = new SemaphoreSlim(1, 1);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// This func is raised periodically according to Period of Timer.
    /// </summary>
    public Func<AsyncPeriodicTimer, CancellationToken, ValueTask> DoWork = (_, _) => ValueTask.CompletedTask;

    /// <summary>
    /// Task period of timer (as milliseconds).
    /// </summary>
    public int Period { get; set; }

    public ValueTask StartAsync(CancellationToken cancellationToken = default)
    {
        if (this.Period <= 0)
        {
            throw new FrameworkException("Period should be set before starting the timer!");
        }

        _timer = new Timer(state => { _ = this.DoWorkCallBack(); }, null, 0, this.Period);

        return ValueTask.CompletedTask;
    }

    public ValueTask StopAsync(CancellationToken cancellationToken = default)
    {
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        _cancellationTokenSource.Cancel();

        return ValueTask.CompletedTask;
    }

    private async Task DoWorkCallBack()
    {
        if (!await _interlock.WaitAsync(this.Period / 2, _cancellationTokenSource.Token))
        {
            _logger.LogInformation("Waited half an execution time, but did not get execution lock.");
            return;
        }

        try
        {
            await this.DoWork(this, _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The timer failed with exception.");
        }
        finally
        {
            _interlock.Release();
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _interlock?.Dispose();
        _cancellationTokenSource?.Dispose();
    }
}
