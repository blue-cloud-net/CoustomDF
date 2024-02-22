using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// Extension methods for all objects.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Used to simplify and beautify casting an object to a type.
    /// </summary>
    /// <typeparam name="T">Type to be casted</typeparam>
    /// <param name="obj">Object to cast</param>
    /// <returns>Casted object</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T As<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }

    /// <summary>
    /// Converts given object to a value type using <see cref="Convert.ChangeType(Object,System.Type)"/> method.
    /// </summary>
    /// <param name="obj">Object to be converted</param>
    /// <typeparam name="T">Type of the target object</typeparam>
    /// <returns>Converted object</returns>
    public static T To<T>(this object obj)
        where T : struct
    {
        if (typeof(T) == typeof(Guid))
        {
            var objString = obj.ToString();
            if (objString.IsNullOrWhiteSpace())
            {
                return default;
            }

            return (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(objString) ?? default!);
        }

        return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Check if an item is in a list.
    /// </summary>
    /// <param name="item">Item to check</param>
    /// <param name="list">List of items</param>
    /// <typeparam name="T">Type of the items</typeparam>
    public static bool IsIn<T>(this T item, params T[] list)
    {
        return list.Contains(item);
    }

    /// <summary>
    /// Check if an item is in the given enumerable.
    /// </summary>
    /// <param name="item">Item to check</param>
    /// <param name="items">Items</param>
    /// <typeparam name="T">Type of the items</typeparam>
    public static bool IsIn<T>(this T item, IEnumerable<T> items)
    {
        return items.Contains(item);
    }
}

public static class ObjectExtensions<T>
    where T : class
{
    private static PropertyInfo[] _prop = typeof(T).GetProperties();

    /// <summary>
    /// 对象转换为字典
    /// </summary>
    /// <param name="obj">待转化的对象</param>
    /// <param name="ignoreNullValue">是否忽略null值</param>
    /// <returns></returns>
    public static Dictionary<string, string> ToMap(T obj, bool ignoreNullValue = true)
    {
        var map = new Dictionary<string, string>();

        foreach (PropertyInfo prop in _prop)
        {
            if (prop.CanRead && prop.GetMethod is not null)
            {
                var value = prop.GetValue(obj);

                if (!ignoreNullValue || value is not null)
                {
                    map.Add(prop.Name, value?.ToString() ?? String.Empty);
                }
            }
        }

        return map;
    }
}
