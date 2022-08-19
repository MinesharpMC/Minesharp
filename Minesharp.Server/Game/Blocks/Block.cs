using Minesharp.Blocks;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Broadcast;
using Minesharp.Server.Game.Entities;
using Minesharp.Server.Game.Enum;
using Minesharp.Server.Game.Worlds;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Worlds;

namespace Minesharp.Server.Game.Blocks;

public sealed class Block : IEquatable<Block>, IBlock
{
    public Position Position { get; init; }
    public World World { get; init; }

    public int BlockType => World.Server.BlockRegistry.GetBlockType(Type);

    public Material Type
    {
        get => World.GetBlockTypeAt(Position);
        set => World.SetBlockTypeAt(Position, value);
    }

    public IWorld GetWorld()
    {
        return World;
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