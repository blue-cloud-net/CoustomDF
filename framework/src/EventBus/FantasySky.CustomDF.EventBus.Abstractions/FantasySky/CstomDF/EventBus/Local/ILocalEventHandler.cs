namespace FantasySky.CstomDF.EventBus.Local;

public interface ILocalEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
{
    /// <summary>
    /// Handler handles the event by implementing this method.
    /// </summary>
    /// <param name="eventData">Event data</param>
    Task HandleEventAsync(TEvent eventData);
}
