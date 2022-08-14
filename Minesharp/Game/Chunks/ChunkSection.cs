using System.Collections.ObjectModel;
using Minesharp.Extension;

namespace Minesharp.Game.Chunks;

public sealed class ChunkSection
{
    private byte bits;
    private long mask;
    private Dictionary<int, long> mapping;
    private int blockCount;
    private readonly IList<int> palette;

    public byte Bits => bits;
    public IEnumerable<long> Mapping => mapping.Values;
    public IEnumerable<int> Palette => palette;
    public int BlockCount => blockCount;

    public ChunkSection() : this(new int[4096])
    {
        
    }
    
    public ChunkSection(IList<int> types)
    {
        this.palette = new List<int>();
        this.bits = palette.GetBitsSize();
        this.mask = (1L << bits) - 1L;
        this.mapping = new Dictionary<int, long>();

        for (var i = 0; i < 4096; i++)
        {
            if (types[i] != 0)
            {
                blockCount++;
            }

            if (!palette.Contains(types[i]))
            {
                palette.Add(types[i]);
            }
            
            mapping.Set(i, bits > 8 ? types[i] : palette.IndexOf(types[i]), bits, mask);
        }
    }

    public int GetType(int x, int y, int z)
    {
        var index = Index(x, y, z);
        var value = mapping.Get(index, bits, mask);
        if (bits > 8)
        {
            return value;
        }

        return palette[value];
    }

    public void SetType(int x, int y, int z, int type)
    {
        var index = Index(x, y, z);
        
        var previous = mapping.Get(index, bits, mask);
        if (previous != 0)
        {
            blockCount--;
        }

        if (type != 0)
        {
            blockCount++;
        }

        int value;
        if (bits > 8)
        {
            value = type;
        }
        else
        {
            value = palette.IndexOf(type);
            if (value == -1)
            {
                palette.Add(type);
                value = palette.IndexOf(type);

                if (value > mask)
                {
                    if (bits == 8)
                    {
                        var modifiedBits = palette.GetBitsSize();
                        var modifiedMask = (1L << bits) - 1L;
                    
                        var modifiedMapping = new Dictionary<int, long>();
                        for (var i = 0; i < 4096; i++)
                        {
                            var oldValue = mapping.Get(i, bits, mask);
                            var newValue = palette.IndexOf(oldValue);
                            
                            modifiedMapping.Set(i, newValue, modifiedBits, modifiedMask);
                        }

                        bits = modifiedBits;
                        mask = modifiedMask;
                        mapping = modifiedMapping;
                        
                        value = type;
                    }
                    else
                    {
                        var modifiedBits = palette.GetBitsSize();
                        var modifiedMask = (1L << bits) - 1L;
                    
                        var modifiedMapping = new Dictionary<int, long>();
                        for (var i = 0; i < 4096; i++)
                        {
                            var oldValue = mapping.Get(i, bits, mask);
                            
                            modifiedMapping.Set(i, oldValue, modifiedBits, modifiedMask);
                        }

                        bits = modifiedBits;
                        mask = modifiedMask;
                        mapping = modifiedMapping;
                    }
                }
            }
        }
        
        mapping.Set(index, value, bits, mask);
    }

    private static int Index(int x, int y, int z)
    {
        return ((y & 0xf) << 8) | (z << 4) | x;
    }
}