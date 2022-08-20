using Minesharp.Blocks;
using Minesharp.Chunks;
using Minesharp.Server.Chunks;
using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Entities;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Storages;
using Minesharp.Server.Worlds;
using Minesharp.Storages;
using Minesharp.Worlds;

namespace Minesharp.Server.Blocks;

public sealed class Block : IEquatable<Block>, IBlock
{
    public GameServer Server => World.Server;
    
    public World World { get; }
    public Position Position { get; }
    public BoundingBox BoundingBox { get; }

    public Chunk Chunk => World.GetChunkAt(Position);

    public int BlockType => World.Server.BlockRegistry.GetBlockIdFromMaterial(Type);

    public Material Type
    {
        get => World.GetBlockTypeAt(Position);
        set => World.SetBlockTypeAt(Position, value);
    }

    public BlockState State => Server.GetBlockFrom(Type).GetState(this);

    public Block(World world, Position position)
    {
        World = world;
        Position = position;
        BoundingBox = BoundingBox.Of(this);
    }
    
    public IWorld GetWorld()
    {
        return World;
    }

    public IChunk GetChunk()
    {
        return Chunk;
    }

    public IBlockState GetState()
    {
        return State;
    }

    public T GetState<T>() where T : class, IBlockState
    {
        return State as T;
    }

    public bool Equals(Block other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Position.Equals(other.Position) && Equals(World, other.World);
    }

    public Block GetRelative(int modX, int modY, int modZ)
    {
        return World.GetBlockAt(Position.BlockX + modX, Position.BlockY + modY, Position.BlockZ + modZ);
    }

    public Block GetRelative(BlockFace face)
    {
        var modifier = face.GetModifiers();
        return GetRelative((int)modifier.X, (int)modifier.Y, (int)modifier.Z);
    }

    public IEnumerable<ItemStack> GetDrops(ItemStack tool = null)
    {
        return Server.GetBlockFrom(Type).GetDrops(tool);
    }

    public void Break()
    {
        if (Type == Material.Air)
        {
            return;
        }

        World.Broadcast(new PlayEffectPacket
        {
            Effect = Effect.BlockBreak,
            Data = BlockType,
            Position = Position,
            IgnoreDistance = false
        }, new CanSeeBlockRule(this), new InRadiusRule(Position, 10));

        Type = Material.Air;
    }

    public void BreakBy(Player player)
    {
        if (Type == Material.Air)
        {
            return;
        }

        World.Broadcast(new PlayEffectPacket
        {
            Effect = Effect.BlockBreak,
            Data = BlockType,
            Position = Position,
            IgnoreDistance = false
        }, new CanSeeBlockRule(this), new InRadiusRule(Position, 10), new ExceptPlayerRule(player));

        Type = Material.Air;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || (obj is Block other && Equals(other));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, World);
    }

    public static bool operator ==(Block left, Block right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Block left, Block right)
    {
        return !Equals(left, right);
    }
}