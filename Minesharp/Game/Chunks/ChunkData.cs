namespace Minesharp.Game.Chunks;

public class ChunkData
{
    public ChunkData()
    {
        Sections = new int[ChunkConstants.SectionCount][];
    }

    public int[][] Sections { get; }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        if (x < 0 || y < 0 || z < 0 || x >= ChunkConstants.Height || y >= ChunkConstants.Depth || z >= ChunkConstants.Width)
        {
            return;
        }

        Sections[y >> 4] ??= new int[4096];
        Sections[y >> 4][((y & 0xF) << 8) | (z << 4) | x] = blockId;
    }

    public void SetBlock(int x, int y, int z, Material material)
    {
        SetBlock(x, y, z, (int)material);
    }
}