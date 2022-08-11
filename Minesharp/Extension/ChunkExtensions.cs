using Minesharp.Game.Chunks;

namespace Minesharp.Extension;

public static class ChunkExtensions
{
    public static int GetHighestSolidBlock(this Chunk chunk, int x, int y, int z)
    {
        for (--y; y >= 0; --y) 
        {
            if (chunk.GetType(x, y, z) != 0) 
            {
                break;
            }
        }
        
        return y + 1;
    }
}