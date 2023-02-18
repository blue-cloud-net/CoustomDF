using FantasySky.CustomDF.Domain.Entities.Specifications;

namespace FantasySky.CustomDF.EntityFrameworkCore;

public static class PageQueryableExtensions
{
    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="query"></param>
    /// <param name="page">分页条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<PagedList<TEntity>> ToPageListAsync<TEntity>(this IQueryable<TEntity> query, Page page, CancellationToken cancellationToken = default)
    {
        Check.IsNotNull(query, nameof(query));

        var sumCount = await query.LongCountAsync();

        var result = query;

        if (!page.Sorting.IsNullOrWhiteSpace())
        {
            result = result.OrderByIf<TEntity, IQueryable<TEntity>>(page.Sorting);
        }

        var entities = await result.PageBy(page.SkipCount, page.MaxCount).ToListAsync();

        return new PagedList<TEntity>(page, entities, sumCount);
    }
}
