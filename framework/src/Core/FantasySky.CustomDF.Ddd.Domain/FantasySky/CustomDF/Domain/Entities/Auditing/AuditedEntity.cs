namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="IAuditedObject"/>.
/// </summary>
[Serializable]
public abstract class AuditedEntity : CreationAuditedEntity, IAuditedObject
{
    /// <inheritdoc />
    public virtual DateTimeOffset? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="IAuditedObject"/>.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedObject
{
    protected AuditedEntity()
    { }

    protected AuditedEntity(TKey id)
            : base(id)
    { }

    /// <inheritdoc />
    public virtual DateTimeOffset? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}
