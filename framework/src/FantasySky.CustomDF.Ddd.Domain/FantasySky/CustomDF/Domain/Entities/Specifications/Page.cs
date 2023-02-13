namespace FantasySky.CustomDF.Domain.Entities.Specifications;

/// <summary>
/// 分页查询
/// </summary>
public struct Page
{
    public Page(
        int pageIndex, int pageSize)
    {
        if (pageIndex <= 0)
        {
            throw new ArgumentException("The pageIndex cannot be less than or equal to 0.", nameof(pageIndex));
        }
        else if (pageSize <= 0)
        {
            throw new ArgumentException("The pageSize cannot be less than or equal to 0.", nameof(pageSize));
        }

        this.PageIndex = pageIndex;
        this.PageSize = pageSize;

        this.SkipCount = (pageIndex - 1) * pageSize;
        this.MaxCount = pageSize;
    }

    public Page(
        int pageIndex, int pageSize, string sorting) : this(pageIndex, pageSize)
    {
        this.Sorting = sorting;
    }

    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; init; }

    /// <summary>
    /// 页容量
    /// </summary>
    public int PageSize { get; init; }

    public string? Sorting { get; set; }

    /// <summary>
    /// 跳过数
    /// </summary>
    public int SkipCount { get; } = 0;

    /// <summary>
    /// 跳过数
    /// </summary>
    public int MaxCount { get; } = Int32.MaxValue;
}
