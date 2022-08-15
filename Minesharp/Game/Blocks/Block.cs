using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Chunks;
using Minesharp.Game.Worlds;

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

    public float GetHardness()
    {
        return Type.GetHardness();
    }

    public void Break()
    {
        if (Type == Material.Air)
        {
            return;
        }

        Type = Material.Air;
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