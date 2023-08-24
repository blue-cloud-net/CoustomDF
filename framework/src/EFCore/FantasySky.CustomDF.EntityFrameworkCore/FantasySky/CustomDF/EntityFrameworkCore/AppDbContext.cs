using FantasySky.CustomDF.Domain.Entities.Auditing;
using FantasySky.CustomDF.Domain.Guids;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FantasySky.CustomDF.EntityFrameworkCore;

public abstract class AppDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    protected AppDbContext(DbContextOptions<TDbContext> options)
    : base(options)
    {
        this.ChangeTracker.Tracked += this.ChangeTracker_Tracked;
        this.ChangeTracker.StateChanged += this.ChangeTracker_StateChanged;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected virtual void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        this.PublishEventsForTrackedEntity(e.Entry);
    }

    protected virtual void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        this.PublishEventsForTrackedEntity(e.Entry);
    }

    private void PublishEventsForTrackedEntity(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                this.ApplyAbpConceptsForAddedEntity(entry);
                //EntityChangeEventHelper.PublishEntityCreatedEvent(entry.Entity);
                break;

            case EntityState.Modified:
                this.ApplyAbpConceptsForModifiedEntity(entry);
                //if (entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
                //{
                //    if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
                //    {
                //        EntityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                //    }
                //    else
                //    {
                //        EntityChangeEventHelper.PublishEntityUpdatedEvent(entry.Entity);
                //    }
                //}

                break;

            case EntityState.Deleted:
                this.ApplyAbpConceptsForDeletedEntity(entry);
                //EntityChangeEventHelper.PublishEntityDeletedEvent(entry.Entity);
                break;
        }
    }

    #region AddTrack

    protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
    {
        this.CheckAndSetId(entry);
        this.SetConcurrencyStampIfNull(entry);
        this.SetCreationAuditProperties(entry);
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId
            && entityWithGuidId.Id == Guid.Empty)
        {
            this.TrySetGuidId(entry, entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
    {
        ObjectHelper.TrySetProperty(
            entity,
            x => x.Id,
            p => SequentialGuidGenerator.NewGuid());
    }

    protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        if (entry.Entity is not IHasConcurrencyStamp entity)
        {
            return;
        }

        if (entity.ConcurrencyStamp != null)
        {
            return;
        }

        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasCreation objectWithCreation)
        {
            return;
        }

        if (objectWithCreation.CreationTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithCreation, x => x.CreationTime, () => DateTimeOffset.UtcNow);
        }

        // TODO 账户Id
    }

    #endregion

    #region UpdateTrack

    protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
        {
            this.IncrementEntityVersionProperty(entry);
            this.SetModificationAuditProperties(entry);

            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                this.SetDeletionAuditProperties(entry);
            }
        }
    }

    protected virtual void IncrementEntityVersionProperty(EntityEntry entry)
    {
        if (entry.Entity is not IHasEntityVersion objectWithModification)
        {
            return;
        }

        if (objectWithModification.EntityVersion == default)
        {
            ObjectHelper.TrySetProperty(objectWithModification, x => x.EntityVersion, x => x.EntityVersion + 1);
        }
    }

    protected virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasModification objectWithModification)
        {
            return;
        }

        if (objectWithModification.LastModificationTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithModification, x => x.LastModificationTime, () => DateTimeOffset.UtcNow);
        }

        // TODO 账户Id
    }

    #endregion

    #region DeleteTrack

    protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
    {
        if (entry.Entity is ISoftDelete softDeleteEntity)
        {
            entry.State = EntityState.Modified;

            foreach (var propertyEntry in entry.Properties)
            {
                propertyEntry.IsModified = false;
            }

            ObjectHelper.TrySetProperty(entry.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
            this.Entry(softDeleteEntity).Property(x => x.IsDeleted).IsModified = true;

            this.SetDeletionAuditProperties(entry);
        }
    }

    protected virtual void SetDeletionAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasDeletion objectWithModification)
        {
            return;
        }

        if (objectWithModification.DeletionTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithModification, x => x.DeletionTime, () => DateTimeOffset.UtcNow);
            this.Entry(objectWithModification).Property(x => x.DeletionTime).IsModified = true;
        }

        // TODO 账户Id
    }

    #endregion
}
