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
    
    private readonly int x;
    private readonly int z;
    private readonly World world;
    
    private List<ChunkSection> sections;
    private sbyte[] heightMap;

    public Chunk(World world, int x, int z)
    {
        this.x = x;
        this.z = z;
        this.world = world;
        this.heightMap = new sbyte[Width * Height];
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

    public World GetWorld()
    {
        return world;
    }

    public int GetType(int blockX, int blockY, int blockZ)
    {
        return GetSection(blockY)?.GetType(blockX, blockY, blockZ) ?? 0;
    }

    public ChunkSection GetSection(int y)
    {
        var index = y >> 4;
        if (y is < 0 or > Depth || index > sections.Count)
        {
            return null;
        }

        return sections[index];
    }

    public IEnumerable<ChunkSection> GetSections()
    {
        return sections;
    }

    public sbyte[] GetHeightmap()
    {
        return heightMap;
    }

    public void Load()
    {
        var generator = world.GetChunkGenerator();
        var data = generator.GenerateChunkData(x, z);

        var dataSections = data.GetSections();
        if (dataSections is not null)
        {
            sections = new List<ChunkSection>();
            for (var i = 0; i < dataSections.Length; ++i)
            {
                var section = dataSections[i];
                if (section is not null)
                {
                    sections.Add(new ChunkSection(section));
                }
            }
        }
    }
}