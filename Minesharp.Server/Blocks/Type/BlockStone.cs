using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Blocks.Type;

public class BlockStone : BlockType
{
    public override Material[] Types { get; } = { Material.Stone };

    public override IEnumerable<ItemStack> GetDrops(ItemStack tool)
    {
        return new[] { new ItemStack(Material.Cobblestone) };
    }
}