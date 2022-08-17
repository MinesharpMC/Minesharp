using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Blocks;

public sealed class Block : IEquatable<Block>
{
    public Position Position { get; init; }
    public World World { get; init; }

    public Material Type
    {
        get => World.GetBlockTypeAt(Position);
        set => World.SetBlockTypeAt(Position, value);
    }

    public Block GetRelative(int modX, int modY, int modZ)
    {
        return World.GetBlockAt(Position.BlockX + modX, Position.BlockY + modY, Position.BlockZ + modZ);
    }

    public Block GetRelative(Face face)
    {
        var modifier = face.GetModifiers();
        return GetRelative((int)modifier.X, (int)modifier.Y, (int)modifier.Z);
    }

    public void Break(Player breaker = null)
    {
        if (Type == Material.Air)
        {
            return;
        }

        World.Broadcast(new PlayEffectPacket
        {
            Effect = Effect.BlockBreak,
            Data = (int)Type,
            Position = Position,
            IgnoreDistance = false,
        }, new CanSeeBlockRule(this), new InRadiusRule(Position, 10), new ExceptPlayerRule(breaker));
        
        Type = Material.Air;
    }

    public void ShowBreakStage(byte stage, Player breaker = null)
    {
        World.Broadcast(new BlockBreakStageUpdatePacket
        {
            EntityId = breaker?.Id ?? World.Random.Next(),
            Stage = stage,
            Position = Position,
        }, new CanSeeBlockRule(this), new InRadiusRule(Position, 10), new ExceptPlayerRule(breaker));
    }

    public void ResetBreakStage(Player breaker = null)
    {
        ShowBreakStage(10, breaker);
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

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Block other && Equals(other);
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