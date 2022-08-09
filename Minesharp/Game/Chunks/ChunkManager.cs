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
            chunks[key] = chunk = new Chunk(world, key.GetX(), key.GetZ());
        }

        return chunk;
    }
}