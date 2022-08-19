namespace Minesharp.Server.Game.Chunks.Generator;

public abstract class ChunkGenerator
{
    private readonly GameServer server;

    protected ChunkGenerator(GameServer server)
    {
        this.server = server;
    }

    public abstract ChunkData Generate(int chunkX, int chunkZ);

    protected ChunkData CreateChunkData()
    {
        return new ChunkData(server);
    }
}