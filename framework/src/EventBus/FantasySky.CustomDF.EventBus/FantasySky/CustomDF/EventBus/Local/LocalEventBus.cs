using FantasySky.CstomDF.EventBus.Local;
using FantasySky.CustomDF.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FantasySky.CustomDF.EventBus.Local;

[Dependency(typeof(ILocalEventBus), ServiceLifetime.Singleton)]
public class LocalEventBus : EventBusBase, ILocalEventBus
{
    public IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        throw new NotImplementedException();
    }
}
