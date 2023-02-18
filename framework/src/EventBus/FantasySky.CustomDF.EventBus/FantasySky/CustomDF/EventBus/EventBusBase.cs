using FantasySky.CstomDF.EventBus;

namespace FantasySky.CustomDF.EventBus;

public class EventBusBase : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }
}
