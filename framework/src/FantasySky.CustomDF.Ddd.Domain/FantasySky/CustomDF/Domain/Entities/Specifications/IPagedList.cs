namespace FantasySky.CustomDF.Domain.Entities.Specifications;

/// <summary>
/// 定义查询分页的结果
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IPagedList<TEntity> where TEntity : IEntity
{
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
    public IEnumerable<TEntity> Items { get; set; }

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
    public int TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }
}

public interface IPagedList<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}
