using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FantasySky.CustomDF;

/// <summary>
/// 检查帮助类
/// </summary>
[DebuggerStepThrough]
public static class Check
{
    [return: NotNull]
    public static T NotDefaultOrNull<T>(
        T? value,
        string parameterName)
        where T : struct
    {
        if (value == null)
        {
            throw new ArgumentException($"{parameterName} is null!", parameterName);
        }

        if (value.Value.Equals(default(T)))
        {
            throw new ArgumentException($"{parameterName} has a default value!", parameterName);
        }

        return value.Value;
    }

    [return: NotNull]
    public static T NotNull<T>(
        [NotNull] T? value,
        string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    [return: NotNull]
    public static T NotNull<T>(
        T? value,
        string parameterName,
        string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, message);
        }

        return value;
    }

    public static string NotNullOrEmpty(
        string? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        return value;
    }

    public static T[] NotNullOrEmpty<T>(
        T[]? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }

    public static IEnumerable<T> NotNullOrEmpty<T>(
        IEnumerable<T>? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }

    public static string NotNullOrWhiteSpace(
        string? value,
        string parameterName)
    {
        if (value.IsNullOrWhiteSpace())
        {
            throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
        }

        return value;
    }
}
