using Minesharp.Extension;

namespace Minesharp.Game.Chunks;

public sealed class ChunkPaletteMapping
{
    public ChunkPaletteMapping(List<int> palette, int[] types)
    {
        Bits = palette.Calculate();
        Bits = Bits < 4 ? (byte)4 : !UsePalette ? (byte)15 : Bits;
        Mask = (1L << Bits) - 1L;
        Storage = new long[(int)Math.Ceiling(Bits * 4096 / 64.0)];

        for (var i = 0; i < 4096; i++)
        {
            Set(i, UsePalette ? palette.IndexOf(types[i]) : types[i]);
        }
    }

    public byte Bits { get; }
    public long[] Storage { get; }
    public long Mask { get; }

    public bool UsePalette => Bits <= 8;

    public int Get(int x, int y, int z)
    {
        return Get(CreateIndex(x, y, z));
    }

    private int Get(int index)
    {
        index *= Bits;
        var i0 = index >> 6;
        var i1 = index & 0x3f;

        var value = Storage[i0] >> i1;
        var i2 = i1 + Bits;

        if (i2 > 64)
        {
            value |= Storage[++i0] << (64 - i1);
        }

        return (int)(value & Mask);
    }

    public void Set(int x, int y, int z, int value)
    {
        Set(CreateIndex(x, y, z), value);
    }

    private void Set(int index, int value)
    {
        index *= Bits;

        var i0 = index >> 6;
        var i1 = index & 0x3f;

        Storage[i0] = (Storage[i0] & ~(Mask << i1)) | ((value & Mask) << i1);

        var i2 = i1 + Bits;
        if (i2 > 64)
        {
            i0++;
            Storage[i0] = (Storage[i0] & -(1L << (i2 - 64))) | (uint)(value >> (64 - i1));
        }
    }

    private static int CreateIndex(int x, int y, int z)
    {
        return ((y & 0xf) << 8) | (z << 4) | x;
    }
}