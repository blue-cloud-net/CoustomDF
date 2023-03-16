using FantasySky.CstomDF.EventBus.Distributed;
using FantasySky.CstomDF.EventBus.Local;

namespace FantasySky.CstomDF.EventBus;

/// <summary>
/// Indirect base interface for all event handlers.
/// Implement <see cref="ILocalEventHandler{TEvent}"/> or <see cref="IDistributedEventHandler{TEvent}"/> instead of this one.
/// </summary>
public interface IEventHandler
{
}
