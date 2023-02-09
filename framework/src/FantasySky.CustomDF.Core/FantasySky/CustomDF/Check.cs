using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FantasySky.CustomDF;

/// <summary>
/// 检查帮助类
/// </summary>
[DebuggerStepThrough]
public static class Check
{
    public static T IsNotDefaultOrNull<T>(
        [NotNull] T? value,
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

    public static T IsNotNull<T>(
            [NotNull] T? value,
        string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    public static T IsNotNull<T>(
        [NotNull] T? value,
        string parameterName,
        string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, message);
        }

        return value;
    }

    public static string IsNotNullOrEmpty(
        [NotNull] string? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        return value;
    }

    public static ICollection<T> IsNotNullOrEmpty<T>(
        [NotNull] ICollection<T>? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }

    public static IEnumerable<T> IsNotNullOrEmpty<T>(
        [NotNull] IEnumerable<T>? value,
        string parameterName)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
        }

        return value;
    }

    public static string IsNotNullOrWhiteSpace(
                [NotNull] string? value,
        string parameterName)
    {
        if (value.IsNullOrWhiteSpace())
        {
            throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
        }

        return value;
    }
}
