namespace Minesharp.Utility;

public class VariableValueArray
{
    private readonly long[] backing;
    private readonly int capacity;
    private readonly int bitsPerValue;
    private readonly long valueMask;

    public int this[int index]
    {
        get
        {
            index *= bitsPerValue;
            var i0 = index >> 6;
            var i1 = index & 0x3f;

            var value = backing[i0] >> i1;
            var i2 = i1 + bitsPerValue;

            if (i2 > 64) 
            {
                value |= backing[++i0] << 64 - i1;
            }

            return (int) (value & valueMask);
        }
        set
        {
            index *= bitsPerValue;
            var i0 = index >> 6;
            var i1 = index & 0x3f;

            backing[i0] = this.backing[i0] & ~(this.valueMask << i1) | (value & valueMask) << i1;
            var i2 = i1 + bitsPerValue;
            if (i2 > 64) 
            {
                i0++;
                backing[i0] = backing[i0] & -(1L << i2 - 64) | (uint)(value >> 64 - i1);
            }
        }
    }

    public int GetBitsPerValue()
    {
        return bitsPerValue;
    }

    public long[] GetBacking()
    {
        return backing;
    }

    public VariableValueArray(int bitsPerValue, int capacity)
    {
        this.backing = new long[(int)Math.Ceiling(bitsPerValue * capacity / 64.0)];
        this.bitsPerValue = bitsPerValue;
        this.capacity = capacity;
        this.valueMask = (1L << bitsPerValue) - 1L;
    }

    public static int Calculate(int size)
    {
        var count = 0;
        do 
        {
            count++;
            size >>= 1;
        } 
        while (size != 0);
        return count;
    }
}