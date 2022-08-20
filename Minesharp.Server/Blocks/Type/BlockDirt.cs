using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Blocks.Type;

public class BlockDirt : BlockType
{
    public override Material[] Types { get; } = { Material.Dirt };
    
    public override IEnumerable<ItemStack> GetDrops(ItemStack tool)
    {
        return new[]
        {
            new ItemStack(Material.Dirt)
        };
    }
}