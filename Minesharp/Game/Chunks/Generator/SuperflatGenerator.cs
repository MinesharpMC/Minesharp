namespace Minesharp.Game.Chunks.Generator;

public class SuperflatGenerator : ChunkGenerator
{
    public override ChunkData Generate(int chunkX, int chunkZ)
    {
        var chunkData = new ChunkData();

        var cx = chunkX << 4;
        var cz = chunkZ << 4;

        for (var x = 0; x < 16; x++)
        for (var z = 0; z < 16; z++)
        {
            GenerateTerrain(chunkData, cx + x, cz + z);
        }

        return chunkData;
    }

    private void GenerateTerrain(ChunkData chunkData, int x, int z)
    {
        x &= 0xF;
        z &= 0xF;

        chunkData.SetBlock(x, 0, z, Material.Bedrock);
        chunkData.SetBlock(x, 1, z, Material.Dirt);
        chunkData.SetBlock(x, 2, z, Material.Dirt);
        chunkData.SetBlock(x, 3, z, Material.GrassBlock);
    }
}