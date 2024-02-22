namespace FantasySky.CustomDF.Domain.Entities;

/// <inheritdoc/>
[Serializable]
public abstract class Entity : IEntity
{
    protected Entity()
    {
        EntityHelper.TrySetTenantId(this);
    }

    public bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }

    public abstract object[] GetKeys();

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[Entity: {this.GetType().Name}] Keys = {this.GetKeys().JoinAsString(", ")}";
    }
}

/// <inheritdoc cref="IEntity{TKey}" />

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey> where TKey : notnull
{
    protected Entity()
    {
        this.Id = default!;
    }

    protected Entity(TKey id)
    {
        this.Id = id;
    }

    /// <inheritdoc/>
    public virtual TKey Id { get; protected set; }

    public override object[] GetKeys()
    {
        return new object[] { this.Id };
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[Entity: {this.GetType().Name}] Id = {this.Id}";
    }
}
