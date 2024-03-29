namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
/// </summary>
[Serializable]
public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
{
    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTimeOffset? DeletionTime { get; set; }

    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
{
    protected FullAuditedEntity()
    {
    }

    protected FullAuditedEntity(TKey id)
            : base(id)
    {
    }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTimeOffset? DeletionTime { get; set; }

    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }
}
