using FantasySky.CustomDF;

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public static class ListExtensions
{
    public static T GetOrAdd<T>([NotNull] this IList<T> source, Func<T, bool> selector, Func<T> factory)
    {
        Check.IsNotNull(source, nameof(source));

        var item = source.FirstOrDefault(selector);

        if (item == null)
        {
            item = factory();
            source.Add(item);
        }

        return item;
    }
}
