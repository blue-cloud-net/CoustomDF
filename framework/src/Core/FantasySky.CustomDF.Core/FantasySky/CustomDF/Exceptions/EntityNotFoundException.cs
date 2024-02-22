namespace FantasySky.CustomDF.Exceptions;

/// <summary>
/// This exception is thrown if an entity excepted to be found but not found.
/// </summary>
public class EntityNotFoundException : FrameworkException
{
    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; set; }

    /// <summary>
    /// Id of the Entity.
    /// </summary>
    public object? Id { get; set; }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType)
        : this(entityType, null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}")
    {
        this.EntityType = entityType;
        this.Id = id;
    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id, Exception innerException)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException)
    {
        this.EntityType = entityType;
        this.Id = id;
    }
}
