using Minesharp.Server.Blocks.State;
using Minesharp.Server.Chunks;
using Minesharp.Server.Storages;
using Minesharp.Server.Worlds;

namespace Minesharp.Server.Blocks.Type;

public abstract class BlockType
{
    public abstract Material[] Types { get; }
    
    public virtual BlockState GetState(Block block)
    {
        return new BlockState(block);
    }

    public virtual IEnumerable<Stack> GetDrops(Stack tool)
    {
        return Array.Empty<Stack>();
    }
}