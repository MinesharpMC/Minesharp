namespace Minesharp.Game.Chunks.Generator;

public abstract class ChunkGenerator
{
    public abstract ChunkData Generate(int chunkX, int chunkZ);
}