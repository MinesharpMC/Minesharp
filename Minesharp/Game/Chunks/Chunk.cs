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
    public ChunkSection[] Sections { get; set; }
    public sbyte[] Heightmap { get; set; }

    public ChunkSection GetSection(int y)
    {
        var index = y >> 4;
        if (y is < 0 or >= Depth || index >= Sections.Length)
        {
            return null;
        }

        return Sections[index];
    }

    public int GetType(int x, int y, int z)
    {
        var section = GetSection(y);
        return section == null ? 0 : section.GetType(x, y, z) >> 4;
    }
}