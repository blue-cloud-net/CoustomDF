namespace FantasySky.CustomDF.Domain.Values;

/// <summary>
/// 通用序号
/// </summary>
public struct CommonNo
{
    public CommonNo(long no)
    {
        this.No = no;
        this.GenerateDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 序号
    /// </summary>
    public long No { get; }

    /// <summary>
    /// 生成日期
    /// </summary>
    public DateTimeOffset GenerateDate { get; }

    public override string ToString()
    {
        return $"{this.GenerateDate.ToCustomShortDate()}{this.No:D5}";
    }

    public string ToString(string stringTemple)
    {
        return String.Format(stringTemple, this.No);
    }
}
