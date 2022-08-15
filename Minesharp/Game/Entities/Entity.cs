using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Worlds;

namespace Minesharp.Game.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public int Id { get; init; }
    public Guid UniqueId { get; init; }
    public Position Position { get; set; }
    public Position LastPosition { get; private set; }
    public Rotation Rotation { get; set; }
    public Rotation LastRotation { get; private set; }
    public World World { get; set; }
    public EntityType Type { get; }

    public bool Moved => Position != LastPosition;
    public bool Rotated => Rotation != LastRotation;

    public Entity(EntityType type)
    {
        Type = type;
    }

    public bool Equals(Entity other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UniqueId.Equals(other.UniqueId);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return UniqueId.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !Equals(left, right);
    }

    public virtual void Tick()
    {
        LastPosition = Position;
        LastRotation = Rotation;
    }
}