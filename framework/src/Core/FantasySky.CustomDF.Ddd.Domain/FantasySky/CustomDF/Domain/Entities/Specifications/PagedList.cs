namespace FantasySky.CustomDF.Domain.Entities.Specifications;

public class PagedList<TEntity> : IPagedList<TEntity>
{
    public PagedList()
    { }

    public PagedList(
        Page page, IEnumerable<TEntity> items, long totalCount)
    {
        this.PageIndex = page.PageIndex;
        this.PageSize = page.PageSize;
        this.Items = items;

        this.TotalCount = totalCount;
        this.TotalPages = (int)Math.Ceiling((double)totalCount / (double)page.PageSize);

        this.HasNextPages = this.PageIndex < this.TotalPages;
        this.HasPrevPages = this.PageIndex - 1 > 0;
    }

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPages { get; set; }

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPrevPages { get; set; }

    /// <summary>
    /// 当前页集合
    /// </summary>
    public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();

    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 页容量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }
}
