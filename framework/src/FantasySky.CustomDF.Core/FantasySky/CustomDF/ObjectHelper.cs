using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace FantasySky.CustomDF;

public static class ObjectHelper
{
    private static readonly ConcurrentDictionary<string, PropertyInfo?> _cachedObjectProperties = new();

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        TrySetProperty(obj, propertySelector, x => valueFactory(), ignoreAttributeTypes);
    }

    public static void TrySetProperty<TObject, TValue>(
        TObject obj,
        Expression<Func<TObject, TValue>> propertySelector,
        Func<TObject, TValue> valueFactory,
        params Type[] ignoreAttributeTypes)
    {
        Check.IsNotNull(obj, nameof(obj));

        var cacheKey = $"{obj.GetType().FullName}-" +
            $"{propertySelector}-" +
            $"{(ignoreAttributeTypes != null ? "-" + String.Join("-", ignoreAttributeTypes.Select(x => x.FullName)) : "")}";

        var property = _cachedObjectProperties.GetOrAdd(cacheKey, key =>
        {
            if (propertySelector.Body.NodeType != ExpressionType.MemberAccess)
            {
                return null;
            }

            var memberExpression = propertySelector.Body.As<MemberExpression>();

            var propertyInfo = obj.GetType().GetProperties().FirstOrDefault(x =>
                x.Name == memberExpression.Member.Name &&
                x.GetSetMethod(true) != null);

            if (propertyInfo == null)
            {
                return null;
            }

            if (ignoreAttributeTypes != null &&
                ignoreAttributeTypes.Any(ignoreAttribute => propertyInfo.IsDefined(ignoreAttribute, true)))
            {
                return null;
            }

            return propertyInfo;
        });

        property?.SetValue(obj, valueFactory(obj));
    }
}
