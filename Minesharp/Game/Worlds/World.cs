using Minesharp.Game.Chunks;
using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public sealed class World
{
    private readonly string name;
    private readonly ChunkManager chunkManager;
    private readonly ChunkGenerator chunkGenerator;

    public World(WorldCreator creator)
    {
        this.name = creator.GetWorldName();
        this.chunkGenerator = creator.GetChunkGenerator();
        this.chunkManager = new ChunkManager(this);
    }

    public ChunkGenerator GetChunkGenerator()
    {
        return chunkGenerator;
    }

    public Chunk GetChunk(ChunkKey chunkKey)
    {
        return chunkManager.GetChunk(chunkKey);
    }

    public string GetName()
    {
        return name;
    }
}