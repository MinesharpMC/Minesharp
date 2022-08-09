namespace Minesharp.Game.Chunks;

public class ChunkData
{
    private readonly int maximumHeight;
    private readonly int[][] sections;

    public ChunkData()
    {
        maximumHeight = 128;
        sections = new int[Chunk.SectionCount][];
    }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        if (x < 0 || y < 0 || z < 0 || x >= Chunk.Height || y >= Chunk.Depth || z >= Chunk.Width) 
        {
            return;
        }
        
        sections[y >> 4] ??= new int[4096];
        sections[y >> 4][(y & 0xF) << 8 | z << 4 | x] = blockId;
    }

    public int[][] GetSections()
    {
        return sections;
    }

    public int GetMaximumHeight()
    {
        return maximumHeight;
    }
}