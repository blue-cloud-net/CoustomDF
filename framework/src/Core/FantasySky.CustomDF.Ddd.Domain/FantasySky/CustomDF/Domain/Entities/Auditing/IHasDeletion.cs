namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// A standard interface to add DeletionTime property to a class.
/// It also makes the class soft delete (see <see cref="ISoftDelete"/>).
/// </summary>
public interface IHasDeletion : ISoftDelete
{
    /// <summary>
    /// Id of the deleter user.
    /// </summary>
    Guid? DeleterId { get; }

    /// <summary>
    /// Deletion time.
    /// </summary>
    DateTimeOffset? DeletionTime { get; }
}
