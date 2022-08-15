using System.Collections.Concurrent;
using Minesharp.Game.Chunks;

namespace Minesharp.Game.Managers;

public class ChunkManager
{
    private readonly ChunkFactory chunkFactory;
    private readonly ConcurrentDictionary<ChunkKey, Chunk> chunks = new();

    public ChunkManager(ChunkFactory chunkFactory)
    {
        this.chunkFactory = chunkFactory;
    }

    public Chunk LoadChunk(ChunkKey key)
    {
        var chunk = GetChunk(key);
        if (chunk is null)
        {
            chunks[key] = chunk = chunkFactory.Create(key);
        }

        return chunk;
    }

    public void UnloadChunk(Chunk chunk)
    {
        if (chunk.IsLocked)
        {
            return;
        }

        chunks.Remove(chunk.Key, out _);
    }

    public Chunk GetChunk(ChunkKey key)
    {
        return chunks.GetValueOrDefault(key);
    }

    public IEnumerable<Chunk> GetChunks()
    {
        return chunks.Values;
    }
}