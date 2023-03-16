using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FantasySky.CustomDF.EntityFrameworkCore.ValueConverters;

public class ArrayToStringValueConverter<T> : ValueConverter<T[], string>
{
    public ArrayToStringValueConverter(
        Func<T, string> objectSerialize,
        Func<string, T> objectDeserialize,
        string split = ";")
        : base(
            d => SerializeObject(d, objectSerialize, split),
            s => DeserializeObject(s, objectDeserialize, split))
    {
        Check.IsNotNullOrWhiteSpace(split, nameof(split));
    }

    private static string SerializeObject(T[] d, Func<T, string> objectSerialize, string split)
    {
        return d.Select(objectSerialize).JoinAsString(split);
    }

    private static T[] DeserializeObject(string s, Func<string, T> objectDeserialize, string split)
    {
        return s.Split(split).Select(objectDeserialize).ToArray();
    }
}
