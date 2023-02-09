using System.Collections.ObjectModel;

using FantasySky.CustomDF.Domain.Uow;

namespace FantasySky.CustomDF.Domain.Entities;

[Serializable]
public abstract class BasicAggregateRoot : Entity,
    IAggregateRoot,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    protected BasicAggregateRoot()
    { }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    { }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}
