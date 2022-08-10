namespace Minesharp.Game.Chunks;

public class ChunkData
{
    public int[][] Sections { get; }

    public ChunkData()
    {
        Sections = new int[Chunk.SectionCount][];
    }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        if (x < 0 || y < 0 || z < 0 || x >= Chunk.Height || y >= Chunk.Depth || z >= Chunk.Width) 
        {
            return;
        }
        
        Sections[y >> 4] ??= new int[4096];
        Sections[y >> 4][(y & 0xF) << 8 | z << 4 | x] = blockId;
    }
}