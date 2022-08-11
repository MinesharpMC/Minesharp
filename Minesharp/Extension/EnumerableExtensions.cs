namespace Minesharp.Extension;

public static class EnumerableExtensions
{
    public static Dictionary<TPrimaryKey, Dictionary<TSecondaryKey, TValue>> ToNestedDictionary<TPrimaryKey,
        TSecondaryKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TPrimaryKey> primarySelector, Func<TValue, TSecondaryKey> secondarySelector)
    {
        var output = new Dictionary<TPrimaryKey, Dictionary<TSecondaryKey, TValue>>();
        foreach (var value in values)
        {
            var primaryKey = primarySelector(value);
            var secondaryKey = secondarySelector(value);
            
            var primary = output.GetValueOrDefault(primaryKey);
            if (primary is null)
            {
                output[primaryKey] = primary = new Dictionary<TSecondaryKey, TValue>();
            }

            primary[secondaryKey] = value;
        }

        return output;
    }

    public static byte CalculateBitsPerEntry(this IEnumerable<int> values)
    {
        var number = values.Count();
        byte count = 0;
        do {
            count++;
            number >>= 1;
        } while (number != 0);
        return count;
    }
}