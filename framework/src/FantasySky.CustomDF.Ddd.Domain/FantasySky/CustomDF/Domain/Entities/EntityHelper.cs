using System.Diagnostics.CodeAnalysis;

using FantasySky.CustomDF.Domain.Entities.MultiTenancy;
using FantasySky.CustomDF.Reflection;

namespace FantasySky.CustomDF.Domain.Entities;

/// <summary>
/// Some helper methods for entities.
/// </summary>
public static class EntityHelper
{
    public static bool IsMultiTenant<TEntity>()
        where TEntity : IEntity
    {
        return IsMultiTenant(typeof(TEntity));
    }

    public static bool IsMultiTenant(Type type)
    {
        return typeof(IMultiTenant).IsAssignableFrom(type);
    }

    public static bool EntityEquals(IEntity entity1, IEntity entity2)
    {
        if (entity1 == null || entity2 == null)
        {
            return false;
        }

        //Same instances must be considered as equal
        if (ReferenceEquals(entity1, entity2))
        {
            return true;
        }

        //Must have a IS-A relation of types or must be same type
        var typeOfEntity1 = entity1.GetType();
        var typeOfEntity2 = entity2.GetType();
        if (!typeOfEntity1.IsAssignableFrom(typeOfEntity2) && !typeOfEntity2.IsAssignableFrom(typeOfEntity1))
        {
            return false;
        }

        //Different tenants may have an entity with same Id.
        if (entity1 is IMultiTenant tenant1 && entity2 is IMultiTenant tenant2)
        {
            var tenant1Id = tenant1.TenantId;
            var tenant2Id = tenant2.TenantId;

            if (tenant1Id != tenant2Id)
            {
                if (tenant1Id == null || tenant2Id == null)
                {
                    return false;
                }

                if (!tenant1Id.Equals(tenant2Id))
                {
                    return false;
                }
            }
        }

        //Transient objects are not considered as equal
        if (HasDefaultKeys(entity1) && HasDefaultKeys(entity2))
        {
            return false;
        }

        var entity1Keys = entity1.GetKeys();
        var entity2Keys = entity2.GetKeys();

        if (entity1Keys.Length != entity2Keys.Length)
        {
            return false;
        }

        for (var i = 0; i < entity1Keys.Length; i++)
        {
            var entity1Key = entity1Keys[i];
            var entity2Key = entity2Keys[i];

            if (entity1Key == null)
            {
                if (entity2Key == null)
                {
                    //Both null, so considered as equals
                    continue;
                }

                //entity2Key is not null!
                return false;
            }

            if (entity2Key == null)
            {
                //entity1Key was not null!
                return false;
            }

            if (TypeHelper.IsDefaultValue(entity1Key) && TypeHelper.IsDefaultValue(entity2Key))
            {
                return false;
            }

            if (!entity1Key.Equals(entity2Key))
            {
                return false;
            }
        }

        return true;
    }

    public static bool HasDefaultKeys([NotNull] IEntity entity)
    {
        Check.IsNotNull(entity, nameof(entity));

        foreach (var key in entity.GetKeys())
        {
            if (!IsDefaultKeyValue(key))
            {
                return false;
            }
        }

        return true;
    }

    public static void TrySetId<TKey>(
        IEntity<TKey> entity,
        Func<TKey> idFactory)
    {
        ObjectHelper.TrySetProperty(
            entity,
            x => x.Id,
            idFactory);
    }

    public static void TrySetTenantId(IEntity entity)
    {
        if (entity is not IMultiTenant multiTenantEntity)
        {
            return;
        }

        var tenantId = AsyncLocalCurrentTenantAccessor.Instance.Current?.TenantId;
        if (tenantId == multiTenantEntity.TenantId)
        {
            return;
        }

        ObjectHelper.TrySetProperty(
            multiTenantEntity,
            x => x.TenantId,
            () => tenantId
        );
    }

    private static bool IsDefaultKeyValue(object value)
    {
        if (value == null)
        {
            return true;
        }

        var type = value.GetType();

        //Workaround for EF Core since it sets int/long to min value when attaching to DbContext
        if (type == typeof(int))
        {
            return Convert.ToInt32(value) <= 0;
        }

        if (type == typeof(long))
        {
            return Convert.ToInt64(value) <= 0;
        }

        return TypeHelper.IsDefaultValue(value);
    }
}
