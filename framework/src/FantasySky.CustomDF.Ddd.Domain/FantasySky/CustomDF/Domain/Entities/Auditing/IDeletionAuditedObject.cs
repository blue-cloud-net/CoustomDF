namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This interface can be implemented to store deletion information (who delete and when deleted).
/// </summary>
public interface IDeletionAuditedObject : IHasDeletion
{ }

/// <summary>
/// Extends <see cref="IDeletionAuditedObject"/> to add user navigation propery.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
public interface IDeletionAuditedObject<TUser> : IDeletionAuditedObject
{
    /// <summary>
    /// Reference to the deleter user.
    /// </summary>
    TUser Deleter { get; }
}
