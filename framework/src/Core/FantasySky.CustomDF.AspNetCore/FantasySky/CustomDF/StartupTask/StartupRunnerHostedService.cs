using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FantasySky.CustomDF.StartupTask;

public class StartupRunnerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public StartupRunnerHostedService(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() => _cancellationTokenSource.Cancel());

        using var scope = _serviceProvider.CreateScope();
        var startupRunner = scope.ServiceProvider.GetRequiredService<IStartupRunner>();
        await startupRunner.StartAsync(_cancellationTokenSource.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();

        return Task.CompletedTask;
    }
}
