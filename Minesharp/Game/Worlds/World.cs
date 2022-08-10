using Minesharp.Game.Chunks;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public sealed class World
{
    public string Name { get; }
    public ChunkManager ChunkManager { get; }
    public ChunkGenerator ChunkGenerator { get; }

    public World(WorldCreator creator)
    {
        Name = creator.Name;
        ChunkGenerator = creator.ChunkGenerator;
        ChunkManager = new ChunkManager(this);
    }

    public Chunk GetChunk(ChunkKey chunkKey)
    {
        return ChunkManager.GetChunk(chunkKey);
    }

    public Chunk GetChunk(int x, int z)
    {
        return ChunkManager.GetChunk(x, z);
    }

    public Chunk LoadChunk(int x, int z)
    {
        return ChunkManager.Load(x, z);
    }

    public Chunk LoadChunk(ChunkKey key)
    {
        return ChunkManager.Load(key);
    }
}