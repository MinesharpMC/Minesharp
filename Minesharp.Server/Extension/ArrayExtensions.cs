using Minesharp.Server.Game.Chunks;

namespace Minesharp.Server.Extension;

public static class ArrayExtensions
{
    public static byte GetBitsSize(this IEnumerable<int> values)
    {
        var number = values.Count();
        byte count = 0;
        do
        {
            count++;
            number >>= 1;
        }
        while (number != 0);

        if (count < 4)
        {
            count = 4;
        }
        else if (count > 8)
        {
            count = 15;
        }

        return count;
    }

    public static void Set(this Dictionary<int, long> mapping, int index, int value, byte bits, long mask)
    {
        index *= bits;
        var i0 = index >> 6;
        var i1 = index & 0x3f;

        if (!mapping.ContainsKey(i0))
        {
            mapping[i0] = 0;
        }

        mapping[i0] = (mapping[i0] & ~(mask << i1)) | ((value & mask) << i1);

        var i2 = i1 + bits;
        if (i2 > 64)
        {
            i0++;
            mapping[i0] = (mapping[i0] & -(1L << (i2 - 64))) | (uint)(value >> (64 - i1));
        }
    }

    public static int Get(this Dictionary<int, long> mapping, int index, byte bits, long mask)
    {
        index *= bits;
        var i0 = index >> 6;
        var i1 = index & 0x3f;

        if (!mapping.ContainsKey(i0))
        {
            mapping[i0] = 0;
        }

        var value = (long)((ulong)mapping[i0] >> i1);
        var i2 = i1 + bits;

        if (i2 > 64)
        {
            value |= mapping[++i0] << (64 - i1);
        }

        return (int)(value & mask);
    }


    public static int GetHighestTypeAt(this Dictionary<int, ChunkSection> sections, int x, int y, int z)
    {
        for (--y; y >= -64; --y)
        {
            var section = sections.GetValueOrDefault(y >> 4);
            var type = (sbyte)(section == null ? 0 : section.GetType(x, y, z) >> 4);
            if (type != 0)
            {
                break;
            }
        }

        return y + 1;
    }
}