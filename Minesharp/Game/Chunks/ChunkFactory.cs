using Minesharp.Extension;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Chunks;

public sealed class ChunkFactory
{
    private readonly ChunkGenerator generator;

    public ChunkFactory(ChunkGenerator generator)
    {
        this.generator = generator;
    }

    public Chunk Create(ChunkKey key)
    {
        var data = generator.Generate(key.X, key.Z);

        var sections = new ChunkSection[data.Sections.Length];
        for (var i = 0; i < data.Sections.Length; i++)
        {
            if (data.Sections[i] is not null)
            {
                sections[i] = new ChunkSection(data.Sections[i]);
            }
        }

        var sy = sections.Length - 1;
        for (; sy >= 0; --sy)
        {
            if (sections.GetSection(sy) != null)
            {
                break;
            }
        }

        var y = (sy + 1) << 4;
        var heightmap = new sbyte[ChunkConstants.Width * ChunkConstants.Height];
        for (var x = 0; x < ChunkConstants.Width; ++x)
        for (var z = 0; z < ChunkConstants.Height; ++z)
        {
            heightmap[z * ChunkConstants.Width + x] = (sbyte)sections.GetHighestTypeAt(x, y, z);
        }

        return new Chunk(key)
        {
            Sections = sections,
            Heightmap = heightmap
        };
    }
}