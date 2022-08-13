namespace Minesharp.Game.Chunks;

public sealed class ChunkSection
{
    private readonly ChunkPaletteMapping mapping;
    private readonly List<int> palette;

    public ChunkSection(int[] types)
    {
        palette = new HashSet<int>(types).ToList();
        mapping = new ChunkPaletteMapping(palette, types);

        BlockCount = types.Count(x => x is not 0);
    }

    public int BlockCount { get; }
    public byte Bits => mapping.Bits;
    public IList<int> Palette => palette;
    public IList<long> Mapping => mapping.Storage;
    public bool UsePalette => mapping.UsePalette;

    public int GetType(int x, int y, int z)
    {
        var value = mapping.Get(x, y, z);
        if (mapping.Bits < 8) // If bits > 8 we use global palette
            value = palette[value];

        return value;
    }
}