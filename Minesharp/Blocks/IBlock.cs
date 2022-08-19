using Minesharp.Entities;
using Minesharp.Worlds;

namespace Minesharp.Blocks;

public interface IBlock
{
    Location Location { get; }
    Material Type { get; set; }
    
    IWorld GetWorld();
}