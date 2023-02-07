namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// A standard interface to add CreationTime and CreatorId property.
/// </summary>
public interface IHasCreation
{
    /// <summary>
    /// Id of the creator.May have.
    /// </summary>
    Guid? CreatorId { get; }
    /// <summary>
    /// Creation time.
    /// </summary>
    DateTimeOffset CreationTime { get; }
}
