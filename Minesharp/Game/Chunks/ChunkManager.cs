using System.Collections.Concurrent;
using Minesharp.Extension;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Chunks;

public class ChunkManager
{
    private readonly ChunkGenerator chunkGenerator;
    private readonly ConcurrentDictionary<ChunkKey, Chunk> chunks = new();

    public ChunkManager(ChunkGenerator chunkGenerator)
    {
        this.chunkGenerator = chunkGenerator;
    }

    public Chunk GetChunk(int x, int z)
    {
        return GetChunk(ChunkKey.Create(x, z));
    }

    public Chunk GetChunk(ChunkKey key)
    {
        return chunks.GetValueOrDefault(key);
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

        chunk = new Chunk
        {
            X = key.X,
            Z = key.Z
        };
        
        var data = chunkGenerator.GenerateChunkData(chunk.X, chunk.Z);

        var sections = new ChunkSection[data.Sections.Length];
        for (var i = 0; i < data.Sections.Length; i++)
        {
            if (data.Sections[i] is not null)
            {
                sections[i] = new ChunkSection(data.Sections[i]);
            }
        }

        chunk.Sections = sections;

        var sy = chunk.Sections.Length - 1;
        for (; sy >= 0; --sy) 
        {
            if (chunk.GetSection(sy) != null) 
            {
                break;
            }
        }
        
        var y = (sy + 1) << 4;
        var heightmap = new sbyte[Chunk.Width * Chunk.Height];
        for (var x = 0; x < Chunk.Width; ++x) 
        {
            for (var z = 0; z < Chunk.Height; ++z)
            {
                heightmap[z * Chunk.Width + x] = (sbyte)chunk.GetHighestSolidBlock(x, y, z);
            }
        }

        chunk.Heightmap = heightmap;

        return chunks[key] = chunk;
    }
}