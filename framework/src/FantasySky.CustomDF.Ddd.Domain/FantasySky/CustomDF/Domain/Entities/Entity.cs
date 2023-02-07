using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasySky.CustomDF.Domain.Entities;

/// <inheritdoc/>
[Serializable]
public abstract class Entity : IEntity
{
    protected Entity()
    {
        EntityHelper.TrySetTenantId(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[Entity: {this.GetType().Name}] Keys = {this.GetKeys().JoinAsString(", ")}";
    }

    public abstract object[] GetKeys();

    public bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

/// <inheritdoc cref="IEntity{TKey}" />

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual TKey Id { get; protected set; }

    //protected Entity()
    //{

    //}

    protected Entity(TKey id)
    {
        this.Id = id;
    }

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
