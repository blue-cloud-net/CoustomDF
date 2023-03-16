using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FantasySky.CustomDF.EntityFrameworkCore.ValueConverters;

public class EnumerableToStringValueConverter<T> : ValueConverter<IEnumerable<T>, string>
{
    public EnumerableToStringValueConverter(
        Func<T, string> objectSerialize,
        Func<string, T> objectDeserialize,
        string split = ";")
        : base(
            d => SerializeObject(d, objectSerialize, split),
            s => DeserializeObject(s, objectDeserialize, split))
    {
        Check.IsNotNullOrWhiteSpace(split, nameof(split));
    }

    private static string SerializeObject(IEnumerable<T> d, Func<T, string> objectSerialize, string split)
    {
        return d.Select(objectSerialize).JoinAsString(split);
    }

    private static IEnumerable<T> DeserializeObject(string s, Func<string, T> objectDeserialize, string split)
    {
        return s.Split(split).Select(objectDeserialize).ToList();
    }
}
