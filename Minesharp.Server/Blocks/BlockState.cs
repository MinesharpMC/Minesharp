using Minesharp.Blocks;
using Minesharp.Chunks;
using Minesharp.Server.Chunks;
using Minesharp.Server.Worlds;
using Minesharp.Worlds;

namespace Minesharp.Server.Blocks;

public class BlockState : IBlockState
{
    public BlockState(Block block)
    {
        World = block.World;
        Position = block.Position;
    }

    public World World { get; }
    public Position Position { get; }
    public Block Block => World.GetBlockAt(Position);
    public Chunk Chunk => World.GetChunkAt(Position);

    public Material Type
    {
        get => World.GetBlockTypeAt(Position);
    }

    public IWorld GetWorld()
    {
        return World;
    }

    public IChunk GetChunk()
    {
        return Chunk;
    }

    public IBlock GetBlock()
    {
        return Block;
    }
}