using Minesharp.Server.Storages;

namespace Minesharp.Server.Blocks.Type;

public class BlockGrass : BlockType
{
    public override Material[] Types { get; } = { Material.GrassBlock };
    
    public override IEnumerable<Stack> GetDrops(Stack tool)
    {
        return new[]
        {
            new Stack(Material.Dirt, 1)
        };
    }
}