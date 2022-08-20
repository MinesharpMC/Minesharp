using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Blocks.Type;

public abstract class BlockType
{
    public abstract Material[] Types { get; }
    
    public virtual BlockState GetState(Block block)
    {
        return new BlockState(block);
    }

    public virtual IEnumerable<ItemStack> GetDrops(ItemStack tool)
    {
        return Array.Empty<ItemStack>();
    }
}