using System.Collections;
using Minesharp.Extension;

namespace Minesharp.Game.Chunks;

public class PaletteWrapper
{
    public byte BitsPerEntry { get; init; }
    public IReadOnlyList<int> Palette { get; init; }
    public BitStorage Storage { get; init; }
    
    public bool HasPalette => Palette is not null;
    
    public static PaletteWrapper Create(int[] types)
    {
        var values = new HashSet<int>(types).ToList();
        var bitsPerEntry = values.CalculateBitsPerEntry();
        switch (bitsPerEntry)
        {
            case < 4:
                bitsPerEntry = 4;
                break;
            case > 8:
                bitsPerEntry = 15;
                values = null;
                break;
        }
        
        var storage = new BitStorage(bitsPerEntry, 4096);
        for (var i = 0; i < 4096; i++)
        {
            storage.Set(i, values?.IndexOf(types[i]) ?? types[i]);
        }

        return new PaletteWrapper
        {
            BitsPerEntry = bitsPerEntry,
            Palette = values,
            Storage = storage
        };
    }

    public int Get(int x, int y, int z)
    {
        var value = Storage.Get(CreateIndex(x, y, z));
        if (Palette is not null)
        {
            value = Palette[value];
        }

        return value;
    }
    
    private static int CreateIndex(int x, int y, int z) 
    {
        return (y & 0xf) << 8 | z << 4 | x;
    }
}