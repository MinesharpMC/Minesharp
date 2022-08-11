using System.Collections;

namespace Minesharp.Game.Chunks;

public class BitStorage
{
    private readonly long[] values;
    private readonly byte bitsPerValue;
    private readonly long mask;

    public int Count => values.Length;
    public IEnumerable<long> Values => values;
    
    public BitStorage(byte bitsPerValue, int capacity)
    {
        this.bitsPerValue = bitsPerValue;
        this.mask = (1L << bitsPerValue) - 1L;
        this.values = new long[(int)Math.Ceiling((bitsPerValue * capacity) / 64.0)];
    }

    public int Get(int index)
    {
        index *= bitsPerValue;
        var i0 = index >> 6;
        var i1 = index & 0x3f;

        var value = values[i0] >> i1;
        var i2 = i1 + bitsPerValue;

        if (i2 > 64) 
        {
            value |= values[++i0] << 64 - i1;
        }

        return (int)(value & mask);
    }

    public void Set(int index, int value)
    {
        index *= bitsPerValue;
        var i0 = index >> 6;
        var i1 = index & 0x3f;

        values[i0] = values[i0] & ~(mask << i1) | (value & mask) << i1;
        
        var i2 = i1 + bitsPerValue;
        if (i2 > 64) 
        {
            i0++;
            values[i0] = values[i0] & -(1L << i2 - 64) | (uint)(value >> 64 - i1);
        }
    }
}