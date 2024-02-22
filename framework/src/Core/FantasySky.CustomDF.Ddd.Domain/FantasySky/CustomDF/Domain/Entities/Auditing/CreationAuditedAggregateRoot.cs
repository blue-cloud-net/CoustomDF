namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
[Serializable]
public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
    /// <inheritdoc />
    public virtual DateTimeOffset CreationTime { get; protected set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; protected set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
    protected CreationAuditedAggregateRoot()
            : base()
    { }

    protected CreationAuditedAggregateRoot(TKey id)
            : base(id)
    { }

    /// <inheritdoc />
    public virtual DateTimeOffset CreationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; set; }
}
