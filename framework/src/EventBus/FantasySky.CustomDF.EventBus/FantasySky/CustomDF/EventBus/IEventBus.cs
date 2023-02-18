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
}
