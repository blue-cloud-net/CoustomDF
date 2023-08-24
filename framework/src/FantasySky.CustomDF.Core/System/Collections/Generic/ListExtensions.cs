using System.Diagnostics.CodeAnalysis;

using FantasySky.CustomDF;

namespace System.Collections.Generic;

/// <summary>
/// Extension methods for <see cref="IList{T}"/>.
/// </summary>
public static class ListExtensions
{
    public static T GetOrAdd<T>([NotNull] this IList<T> source, Func<T, bool> selector, Func<T> factory)
    {
        Check.NotNull(source, nameof(source));

        var item = source.FirstOrDefault(selector);

        if (item == null)
        {
            item = factory();
            source.Add(item);
        }

        return item;
    }
}
