using System.ComponentModel;
using System.Reflection;

namespace System;

/// <summary>
/// <see cref="Enum"/>的扩展方法
/// </summary>
public static class EnumExtensions
{
    public static string GetName<T>(this T @enum) where T : struct, Enum
    {
        return Enum.GetName(@enum)
            ?? throw new InvalidDataException("Get the enum name failed.");
    }

    public static string? GetEnumName<T>(this T @enum) where T : Enum
    {
        return Enum.GetName(@enum.GetType(), @enum);
    }

    public static string? GetEnumDisplayName<T>(this T @enum) where T : Enum
    {
        var fileds = @enum.GetType().GetFields();

        var filed = fileds.FirstOrDefault(p => p.Name == @enum.GetEnumName())
            ?? throw new InvalidOperationException("The enum value can not found.");

        var descAttribute = filed.GetCustomAttribute<DescriptionAttribute>();

        var result = descAttribute?.Description;

        return result;
    }
}