namespace FantasySky.CustomDF.Domain.Entities.Auditing;

/// <summary>
/// This interface can be implemented to store deletion information (who delete and when deleted).
/// </summary>
public interface IDeletionAuditedObject : IHasDeletion
{ }
