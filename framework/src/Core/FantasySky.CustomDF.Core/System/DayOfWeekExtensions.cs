namespace System;

/// <summary>
/// <see cref="DayOfWeek"/>的扩展方法
/// </summary>
public static class DayOfWeekExtensions
{
    /// <summary>
    /// 检查<see cref="DayOfWeek"/>值是否为周末，即周六或周日.
    /// </summary>
    public static bool IsWeekend(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek.IsIn(DayOfWeek.Saturday, DayOfWeek.Sunday);
    }

    /// <summary>
    /// 检查<see cref="DayOfWeek"/>值是否为周一到周五.
    /// </summary>
    public static bool IsWeekday(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek.IsIn(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday);
    }
}