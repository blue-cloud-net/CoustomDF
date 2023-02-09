namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This interface can be implemented to store modification information (who and when modified lastly).
/// </summary>
public interface IModificationAuditedObject : IHasModification
{

}

/// <summary>
/// Adds navigation properties to <see cref="IModificationAuditedObject"/> interface for a user.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
public interface IModificationAuditedObject<TUser> : IModificationAuditedObject
{
    /// <summary>
    /// Reference to the last modifier user of this entity.
    /// </summary>
    TUser LastModifier { get; }
}