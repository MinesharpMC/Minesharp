using Minesharp.Server.Blocks.State;
using Minesharp.Server.Worlds;

namespace Minesharp.Server.Blocks.Type;

public class BlockChest : BlockType
{
    public override Material[] Types { get; } = { Material.Chest };

    public override BlockState GetState(Block block)
    {
        return new Chest(block);
    }
}