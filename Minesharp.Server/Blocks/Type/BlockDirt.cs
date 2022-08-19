using Minesharp.Server.Storages;

namespace Minesharp.Server.Blocks.Type;

public class BlockDirt : BlockType
{
    public override Material[] Types { get; } = { Material.Dirt };
    
    public override IEnumerable<Stack> GetDrops(Stack tool)
    {
        return new[]
        {
            new Stack(Material.Dirt, 1)
        };
    }
}