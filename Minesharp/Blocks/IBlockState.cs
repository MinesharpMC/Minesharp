using Minesharp.Chunks;
using Minesharp.Worlds;

namespace Minesharp.Blocks;

public interface IBlockState
{
    Material Type { get; }

    IWorld GetWorld();
    IChunk GetChunk();
    IBlock GetBlock();
}