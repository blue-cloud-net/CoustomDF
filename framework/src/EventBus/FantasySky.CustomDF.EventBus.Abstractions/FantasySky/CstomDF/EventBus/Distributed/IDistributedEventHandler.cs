namespace FantasySky.CstomDF.EventBus.Distributed;

public interface IDistributedEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
{
    /// <summary>
    /// Handler handles the event by implementing this method.
    /// </summary>
    /// <param name="eventData">Event data</param>
    Task HandleEventAsync(TEvent eventData);
}
