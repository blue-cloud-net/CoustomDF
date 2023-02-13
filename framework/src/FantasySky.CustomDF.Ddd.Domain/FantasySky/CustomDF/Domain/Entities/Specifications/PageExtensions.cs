namespace FantasySky.CustomDF.Domain.Entities.Specifications;

public static class PageExtensions
{
    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="page">分页条件</param>
    /// <returns></returns>
    public static IQueryable<T> PageByIf<T>(this IQueryable<T> query, Page? page)
    {
        Check.IsNotNull(query, nameof(query));

        var temp = query;

        if (page is not null)
        {
            if (!page.Value.Sorting.IsNullOrWhiteSpace())
            {
                temp = temp.OrderByIf<T, IQueryable<T>>(page.Value.Sorting);
            }

            temp = temp.PageBy(page.Value.SkipCount, page.Value.MaxCount);
        }

        return temp;
    }
}
