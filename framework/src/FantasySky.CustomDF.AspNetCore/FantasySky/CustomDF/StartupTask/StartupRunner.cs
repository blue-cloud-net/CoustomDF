using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FantasySky.CustomDF.StartupTask;

[Dependency(typeof(IStartupRunner))]
public class StartupRunner : IStartupRunner
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StartupRunner> _logger;
    private readonly ICollection<Type> _startupTaskTypes;

    public StartupRunner(
        IEnumerable<IStartupTask> startupTasks,
        IServiceScopeFactory scopeFactory,
        ILogger<StartupRunner> logger)
    {
        _scopeFactory = scopeFactory;

        _logger = logger;
        _startupTaskTypes = startupTasks.OrderBy(x => x.Order).Select(x => x.GetType()).ToList();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var startupTaskType in _startupTaskTypes)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var startupTask = (IStartupTask)scope.ServiceProvider.GetRequiredService(startupTaskType);
            _logger.LogInformation($"Running startup task {startupTaskType.Name}");

            try
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"The startup task '{startupTaskType.Name}' execute failed.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
