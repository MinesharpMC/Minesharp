using Minesharp.Utility;

namespace Minesharp.Game.Chunks;

public class ChunkSection
{
    private List<int> palette;
    private VariableValueArray data;

    public ChunkSection(int[] types)
    {
        Load(types);
    }

    public VariableValueArray GetData()
    {
        return data;
    }

    public IReadOnlyList<int> GetPalette()
    {
        return palette;
    }

    public int GetType(int x, int y, int z) 
    {
        var value = data[CreateIndex(x, y, z)];
        if (palette != null) 
        {
            value = palette[value];
        }
        
        return value;
    }

    private int CreateIndex(int x, int y, int z)
    {
        return (y & 0xf) << 8 | z << 4 | x;
    }
    
    private void Load(int[] types)
    {
        palette = new List<int>();
        foreach (var type in types)
        {
            if (!palette.Contains(type))
            {
                palette.Add(type);
            }
        }
        
        var bitsPerBlock = VariableValueArray.Calculate(palette.Count);
        switch (bitsPerBlock)
        {
            case < 4:
                bitsPerBlock = 4;
                break;
            case > 8:
                palette = null;
                bitsPerBlock = 15;
                break;
        }

        data = new VariableValueArray(bitsPerBlock, 4096);
        for (var i = 0; i < 4096; i++)
        {
            if (palette != null)
            {
                data[i] = palette.LastIndexOf(types[i]);
            }
            else
            {
                data[i] = types[i];
            }
        }
    }
}