using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FantasySky.CustomDF.EntityFrameworkCore.ValueComparers;

public class EnumerableValueComparer<T> : ValueComparer<IEnumerable<T>>
    where T : notnull
{
    public EnumerableValueComparer()
        : base(
            (e1, e2) => (e1 == null && e2 == null) || (e1 != null && e2 != null && e1.SequenceEqual(e2)),
            e => e.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            d => d.ToList())
    {
    }
}
