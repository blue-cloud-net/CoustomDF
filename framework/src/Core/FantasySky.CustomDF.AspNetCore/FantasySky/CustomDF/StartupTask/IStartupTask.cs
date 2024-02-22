namespace FantasySky.CustomDF.StartupTask;

public interface IStartupTask
{
    int Order { get; }

    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
