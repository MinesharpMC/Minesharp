namespace Minesharp.Game.Chunks.Generator;

public abstract class ChunkGenerator
{
    private readonly Server server;

    protected ChunkGenerator(Server server)
    {
        this.server = server;
    }

    public abstract ChunkData Generate(int chunkX, int chunkZ);

    protected ChunkData CreateChunkData()
    {
        return new ChunkData(server);
    }
}