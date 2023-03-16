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

    public IDisposable Subscribe<TEvent, THandler>()
        where TEvent : class
        where THandler : IEventHandler, new()
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        throw new NotImplementedException();
    }

    public void UnsubscribeAll<TEvent>() where TEvent : class
    {
        throw new NotImplementedException();
    }
}
