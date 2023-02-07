using FantasySky.CustomDF.Domain.Entities.Auditing;

namespace FantasySky.CustomDF.Domain.Entities;

[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot,
    IHasConcurrencyStamp
{
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        this.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
    IHasConcurrencyStamp
{

    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        this.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
}
