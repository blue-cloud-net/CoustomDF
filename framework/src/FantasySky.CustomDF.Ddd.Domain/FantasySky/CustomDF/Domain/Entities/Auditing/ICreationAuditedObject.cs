namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This interface can be implemented to store creation information (who and when created).
/// </summary>
public interface ICreationAuditedObject : IHasCreation
{

}

/// <summary>
/// Adds navigation property (object reference) to <see cref="ICreationAuditedObject"/> interface.
/// </summary>
/// <typeparam name="TCreator">Type of the user</typeparam>
public interface ICreationAuditedObject<TCreator> : ICreationAuditedObject
{

    /// <summary>
    /// Reference to the creator.
    /// </summary>
    TCreator? Creator { get; }
}
