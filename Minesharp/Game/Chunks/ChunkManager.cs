using System.Collections.Concurrent;
using Minesharp.Game.Worlds;

namespace Minesharp.Game.Chunks;

public class ChunkManager
{
    private readonly World world;
    private readonly ConcurrentDictionary<ChunkKey, Chunk> chunks = new();

    public ChunkManager(World world)
    {
        this.world = world;
    }

    public Chunk GetChunk(int x, int z)
    {
        return GetChunk(ChunkKey.Create(x, z));
    }

    public Chunk GetChunk(ChunkKey key)
    {
        var chunk = chunks.GetValueOrDefault(key);
        if (chunk is null)
        {
            chunks[key] = chunk = new Chunk(world)
            {
                X = key.X,
                Z = key.Z
            };
        }

        return chunk;
    }

    public Chunk Load(int x, int z)
    {
        return Load(ChunkKey.Create(x, z));
    }
    
    public Chunk Load(ChunkKey key)
    {
        var chunk = chunks.GetValueOrDefault(key);
        if (chunk is not null)
        {
            return chunk;
        }

        chunk = new Chunk(world)
        {
            X = key.X,
            Z = key.Z
        };
        
        var generator = world.ChunkGenerator;
        var data = generator.GenerateChunkData(chunk.X, chunk.Z);

        chunk.Sections = (from value 
            in data.Sections 
            where value is not null 
            select new ChunkSection(value))
            .ToList();

        var sy = chunk.Sections.Count - 1;
        for (; sy >= 0; --sy) 
        {
            if (chunk.Sections[sy] != null) 
            {
                break;
            }
        }
        
        var y = (sy + 1) << 4;
        var section = chunk.GetSection(y);
        var heightmap = new sbyte[Chunk.Width * Chunk.Height];
        for (var x2 = 0; x2 < Chunk.Width; ++x2) 
        {
            for (var z2 = 0; z2 < Chunk.Height; ++z2)
            {
                heightmap[z2 * Chunk.Width + x2] = (sbyte)section.GetHighestNonZeroType(x2, y, z2);
            }
        }

        chunk.Heightmap = heightmap;

        return chunks[key] = chunk;
    }
}