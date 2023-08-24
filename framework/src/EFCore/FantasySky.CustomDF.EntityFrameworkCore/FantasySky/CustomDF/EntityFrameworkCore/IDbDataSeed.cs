namespace FantasySky.CustomDF.EntityFrameworkCore;

public interface IDbDataSeed<TEntity> where TEntity : IEntity
{
    static abstract IEnumerable<TEntity> GetSeedData();
}
