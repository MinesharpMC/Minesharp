using Minesharp.Entities;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Entities.Meta;
using Minesharp.Server.Game.Worlds;
using Minesharp.Server.Network.Packet.Game;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Worlds;

namespace Minesharp.Server.Game.Entities;

public abstract class Entity : IEquatable<Entity>, IEntity
{
    public Position Position { get; set; }
    public World World { get; set; }
    public Vector Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public Position LastPosition { get; set; }
    public Rotation LastRotation { get; set; }
    public GameServer Server { get; init; }
    public long TicksLived { get; set; }
    public MetadataRegistry Metadata { get; } = new();

    public bool Moved => Position != LastPosition;
    public bool Rotated => Rotation != LastRotation;
    public int Id { get; init; }
    public Guid UniqueId { get; init; }

    public Location Location =>
        new()
        {
            Position = Position,
            World = World
        };

    public Rotation Rotation { get; set; }

    public IWorld GetWorld()
    {
        return World;
    }
    
    public abstract void Tick();

    public abstract void Update();

    public virtual IEnumerable<GamePacket> GetSpawnPackets()
    {
        return Array.Empty<GamePacket>();
    }

    public virtual IEnumerable<GamePacket> GetUpdatePackets()
    {
        var packets = new List<GamePacket>();

        var changes = Metadata.GetChanges();
        if (changes.Any())
        {
            packets.Add(new EntityMetadataPacket(Id, changes));
        }

        var delta = Position.Delta(LastPosition);
        var teleport = delta.X > short.MaxValue || delta.Y > short.MaxValue || delta.Z > short.MaxValue
                       || delta.X < short.MinValue || delta.Y < short.MinValue || delta.Z < short.MinValue;

        switch (Moved)
        {
            case true when teleport:
                packets.Add(new EntityTeleportPacket(Id, Position, Rotation, IsGrounded));
                break;
            case true when Rotated:
                packets.Add(new UpdateEntityPositionAndRotationPacket(Id, delta, Rotation, IsGrounded));
                packets.Add(new HeadRotationPacket(Id, Rotation.GetIntYaw()));
                break;
            case true:
                packets.Add(new UpdateEntityPositionPacket(Id, delta, IsGrounded));
                break;
            case false when Rotated:
                packets.Add(new UpdateEntityRotationPacket(Id, Rotation, IsGrounded));
                packets.Add(new HeadRotationPacket(Id, Rotation.GetIntYaw()));
                break;
        }

        return packets;
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

        if (obj.GetType() != GetType())
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
}