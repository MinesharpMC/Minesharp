using Minesharp.Utility;

namespace Minesharp.Game.Chunks;

public class ChunkSection
{
    public short BlockCount { get; }
    public PaletteWrapper State { get; }

    public ChunkSection(int[] types)
    {
        BlockCount = (short) types.Count(x => x != 0);
        State = PaletteWrapper.Create(types);
    }
    
    public int GetType(int x, int y, int z)
    {
        return State.Get(x, y, z);
    }
}