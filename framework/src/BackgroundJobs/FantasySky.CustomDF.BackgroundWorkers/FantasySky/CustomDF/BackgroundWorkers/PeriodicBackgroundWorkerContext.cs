namespace FantasySky.CustomDF.BackgroundWorkers;

public class PeriodicBackgroundWorkerContext
{
    public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
        this.CancellationToken = default;
    }

    public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        this.ServiceProvider = serviceProvider;
        this.CancellationToken = cancellationToken;
    }

    public IServiceProvider ServiceProvider { get; }

    public CancellationToken CancellationToken { get; }
}
