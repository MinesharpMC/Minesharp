using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public class WorldCreator
{
    private readonly string name;
    
    private ChunkGenerator chunkGenerator;

    public WorldCreator(string name)
    {
        this.name = name;
    }

    public WorldCreator WithChunkGenerator(ChunkGenerator generator)
    {
        this.chunkGenerator = generator;
        return this;
    }

    public string GetWorldName()
    {
        return name;
    }

    public ChunkGenerator GetChunkGenerator()
    {
        return chunkGenerator;
    }
}