using FantasySky.CstomDF.EventBus;

namespace FantasySky.CustomDF.EventBus;

public interface IEventBus
{
    /// <summary>
    /// Triggers an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true)
        where TEvent : IEvent;

    /// <summary>
    /// Registers to an event.
    /// Given action is called for all event occurrences.
    /// </summary>
    /// <param name="action">Action to handle events</param>
    /// <typeparam name="TEvent">Event type</typeparam>
    IDisposable Subscribe<TEvent>(Func<TEvent, Task> action)
        where TEvent : IEvent;

    /// <summary>
    /// Registers to an event.
    /// A new instance of <typeparamref name="THandler"/> object is created for every event occurrence.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <typeparam name="THandler">Type of the event handler</typeparam>
    IDisposable Subscribe<TEvent, THandler>()
        where TEvent : class
        where THandler : IEventHandler, new();

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="action"></param>
    void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        where TEvent : class;

    /// <summary>
    /// Unregisters all event handlers of given event type.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    void UnsubscribeAll<TEvent>()
        where TEvent : class;
}
