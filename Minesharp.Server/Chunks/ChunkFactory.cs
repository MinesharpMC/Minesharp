using Minesharp.Server.Chunks.Generator;
using Minesharp.Server.Extension;
using Minesharp.Server.Worlds;

namespace Minesharp.Server.Chunks;

public sealed class ChunkFactory
{
    private readonly World world;
    private readonly ChunkGenerator generator;

    public ChunkFactory(ChunkGenerator generator, World world)
    {
        this.generator = generator;
        this.world = world;
    }

    public Chunk Create(ChunkKey key)
    {
        var data = generator.Generate(key.X, key.Z);

        var sections = new Dictionary<int, ChunkSection>();
        foreach (var (sectionId, sectionData) in data.Sections)
        {
            sections[sectionId] = new ChunkSection(sectionData);
        }

        var sy = 16;
        for (; sy >= -4; --sy)
        {
            if (sections.GetValueOrDefault(sy) != null)
            {
                break;
            }
        }

        var y = (sy + 1) << 4;
        var heightmap = new sbyte[16 * 16];
        for (var x = 0; x < 16; ++x)
        for (var z = 0; z < 16; ++z)
        {
            heightmap[z * 16 + x] = (sbyte)sections.GetHighestTypeAt(x, y, z);
        }

        return new Chunk(key, world)
        {
            Sections = sections,
            Heightmap = heightmap
        };
    }
}