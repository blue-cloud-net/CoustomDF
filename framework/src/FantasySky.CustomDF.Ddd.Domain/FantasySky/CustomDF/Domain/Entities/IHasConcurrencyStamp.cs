namespace FantasySky.CustomDF.Domain.Entities;

public interface IHasConcurrencyStamp
{
    /// <summary>
    /// 用于控制乐观并发用
    /// </summary>
    string ConcurrencyStamp { get; set; }
}
