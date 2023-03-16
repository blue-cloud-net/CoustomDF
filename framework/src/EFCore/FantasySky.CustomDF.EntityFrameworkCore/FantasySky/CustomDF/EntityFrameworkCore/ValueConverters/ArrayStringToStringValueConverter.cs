namespace FantasySky.CustomDF.EntityFrameworkCore.ValueConverters;

public class ArrayStringToStringValueConverter : ArrayToStringValueConverter<string>
{
    public ArrayStringToStringValueConverter()
        : base(
            d => d,
            s => s)
    {
    }

    public static ArrayStringToStringValueConverter Instance = new();
}
