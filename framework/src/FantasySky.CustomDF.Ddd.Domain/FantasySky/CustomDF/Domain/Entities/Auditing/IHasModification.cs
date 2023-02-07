namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// A standard interface to add DeletionTime and LastModificationTime property to a class.
/// </summary>
public interface IHasModification
{
    /// <summary>
    /// Last modifier user for this entity.
    /// </summary>
    Guid? LastModifierId { get; }
    /// <summary>
    /// The last modified time for this entity.
    /// </summary>
    DateTimeOffset? LastModificationTime { get; }
}
