using Minesharp.Game.Worlds;

namespace Minesharp.Game.Chunks;

public class Chunk
{
    public const int Width = 16;
    public const int Height = 16;
    public const int Depth = 256;
    public const int SectionDepth = 16;
    public const int SectionCount = Depth / SectionDepth;
    
    public static readonly byte[] EmptyLight = new byte[2048];
    
    public int X { get; init; }
    public int Z { get; init; }
    public List<ChunkSection> Sections { get; set; }
    public sbyte[] Heightmap { get; set; }
    public World World { get; }
    
    public Chunk(World world)
    {
        World = world;
    }

    public ChunkSection GetSection(int y)
    {
        var index = y >> 4;
        if (y is < 0 or > Depth || index > Sections.Count)
        {
            return null;
        }

        return Sections[index];
    }
}