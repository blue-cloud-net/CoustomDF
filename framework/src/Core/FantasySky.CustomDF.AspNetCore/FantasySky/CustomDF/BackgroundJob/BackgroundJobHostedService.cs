using FantasySky.CustomDF.BackgroundWorkers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.BackgroundJob;

public class BackgroundJobHostedService : IHostedService
{
    private readonly ILogger<BackgroundJobHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public BackgroundJobHostedService(
        ILogger<BackgroundJobHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => _cancellationTokenSource.Cancel());

        // 长期后台任务，无需释放
        var scope = _serviceProvider.CreateScope();
        var backgroundWorkers = scope.ServiceProvider.GetServices<IBackgroundWorker>();

        foreach (var backgroundWorker in backgroundWorkers)
        {
            try
            {
                await backgroundWorker.StartAsync(_cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"The background job '{backgroundWorker.GetType().Name}' start failed.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();

        return Task.CompletedTask;
    }
}
