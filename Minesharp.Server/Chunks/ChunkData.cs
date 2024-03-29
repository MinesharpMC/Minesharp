namespace Minesharp.Server.Chunks;

public class ChunkData
{
    private readonly GameServer server;

    public ChunkData(GameServer server)
    {
        this.server = server;

        Sections = new Dictionary<int, int[]>();
    }

    public Dictionary<int, int[]> Sections { get; }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        if (x < 0 || y < -64 || z < 0 || x >= 16 || y >= 256 || z >= 16)
        {
            return;
        }

        var section = Sections.GetValueOrDefault(y >> 4);
        if (section is null)
        {
            Sections[y >> 4] = section = new int[4096];
        }

        section[((y & 0xF) << 8) | (z << 4) | x] = blockId;
    }

    public void SetBlock(int x, int y, int z, Material material)
    {
        if (!material.IsBlock)
        {
            return;
        }

        var blockId = server.BlockRegistry.GetBlockIdFromMaterial(material);
        SetBlock(x, y, z, blockId);
    }
}