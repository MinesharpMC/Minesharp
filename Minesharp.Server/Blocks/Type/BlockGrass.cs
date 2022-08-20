using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Blocks.Type;

public class BlockGrass : BlockType
{
    public override Material[] Types { get; } = { Material.GrassBlock };
    
    public override IEnumerable<ItemStack> GetDrops(ItemStack tool)
    {
        return new[]
        {
            new ItemStack(Material.Dirt)
        };
    }
}