using FantasySky.CustomDF;
using FantasySky.CustomDF.Domain.Entities.Auditing;
using FantasySky.CustomDF.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class EntityTypeBuilderExtensions
{
    public static void HasSeedData<TEntity, T>(this EntityTypeBuilder<TEntity> builder)
        where T : IDbDataSeed<TEntity>
        where TEntity : class, IEntity
    {
        var data = T.GetSeedData();

        data = data.Select(p =>
        {
            if (p is IHasCreation creationEntity
                && creationEntity.CreationTime == default)
            {
                ObjectHelper.TrySetProperty(creationEntity, x => x.CreationTime, () => DateTimeOffset.UtcNow);
            }

            return p;
        });

        builder.HasData(data);
    }
}
