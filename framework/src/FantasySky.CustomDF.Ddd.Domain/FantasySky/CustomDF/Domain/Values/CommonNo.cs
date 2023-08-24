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

        this.ToStringFunction = x => $"{x.GenerateDate.ToCustomShortDate()}{x.No:D5}";
        ;
    }

    public CommonNo(long no, Func<CommonNo, string> toStringFunction)
    {
        this.No = no;
        this.GenerateDate = DateTimeOffset.UtcNow;

        this.ToStringFunction = toStringFunction;
    }

    public Func<CommonNo, string> ToStringFunction { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public long No { get; }

    /// <summary>
    /// 生成日期
    /// </summary>
    public DateTimeOffset GenerateDate { get; }

    public override string ToString() => this.ToStringFunction(this);

    public string ToString(string stringTemple)
    {
        return String.Format(stringTemple, this.No);
    }
}
