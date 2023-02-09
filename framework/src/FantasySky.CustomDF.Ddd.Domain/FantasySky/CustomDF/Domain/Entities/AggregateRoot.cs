using FantasySky.CustomDF.Domain.Entities.Auditing;

namespace FantasySky.CustomDF.Domain.Entities;

[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot,
    IHasConcurrencyStamp
{
    protected AggregateRoot()
    {
        this.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
    IHasConcurrencyStamp
{
    protected AggregateRoot()
        : base()
    {
        this.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        this.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }
}
