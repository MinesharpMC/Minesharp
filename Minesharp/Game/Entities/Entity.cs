using Minesharp.Common;
using Minesharp.Common.Extension;
using Minesharp.Game.Entities.Meta;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public int Id { get; init; }
    public Guid UniqueId { get; init; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    public World World { get; set; }
    public Vector Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public Position LastPosition { get; set; }
    public Rotation LastRotation { get; set; }
    public Server Server { get; init; }
    public long TicksLived { get; set; }
    protected MetadataRegistry Metadata { get; } = new();

    public bool Moved => Position != LastPosition;
    public bool Rotated => Rotation != LastRotation;

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
}