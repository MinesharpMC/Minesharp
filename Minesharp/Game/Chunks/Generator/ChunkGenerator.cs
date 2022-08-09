namespace Minesharp.Game.Chunks.Generator;

public abstract class ChunkGenerator
{
    public abstract ChunkData GenerateChunkData(int chunkX, int chunkZ);
}