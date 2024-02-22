namespace System;

public static class DateTimeExtensions
{
    public static string ToCustomShortDate(this DateTime dateTime) => dateTime.ToString("yyyyMMdd");

    public static string ToCustomShortDate(this DateTimeOffset dateTime) => dateTime.ToString("yyyyMMdd");

    public static string ToCustomShortTime(this DateTime dateTime) => dateTime.ToString("HHmmss");

    public static string ToCustomShortTime(this DateTimeOffset dateTime) => dateTime.ToString("HHmmss");

    public static string ToCustomShortDateTime(this DateTime dateTime) => dateTime.ToString("yyyyMMddHHmmss");

    public static string ToCustomShortDateTime(this DateTimeOffset dateTime) => dateTime.ToString("yyyyMMddHHmmss");

    /// <summary>
    /// 清除时间，只保留日期
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime ClearTime(this DateTime dateTime)
    {
        return dateTime.Subtract(
            new TimeSpan(
                0,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond
            )
        );
    }
}
