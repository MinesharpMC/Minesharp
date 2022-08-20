using Minesharp.Blocks;
using Minesharp.Entities;
using Minesharp.Server.Entities.Metadata;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Worlds;
using Minesharp.Worlds;

namespace Minesharp.Server.Entities;

public abstract class Entity : IEquatable<Entity>, IEntity
{
    public World World { get; set; }
    public Vector Velocity { get; set; }
    public bool IsGrounded { get; set; }
    public Position LastPosition { get; set; }
    public Rotation LastRotation { get; set; }
    public GameServer Server { get; }
    public long TicksLived { get; set; }
    public MetadataRegistry Metadata { get; } = new();
    
    protected Vector Gravity { get; set; }

    public bool Moved => Position != LastPosition;
    public bool Rotated => Rotation != LastRotation;
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    
    public double Height { get; protected set; }
    public double Width { get; protected set; }

    public int Id { get; }
    public Guid UniqueId { get; }
    
    public BoundingBox BoundingBox => BoundingBox.Of(this);

    public Entity(World world, Position position)
    {
        World = world;
        Server = World.Server;
        Id = Server.GetNextEntityId();
        UniqueId = Guid.NewGuid();
        Position = position;
        LastPosition = position;
    }
    
    public IWorld GetWorld()
    {
        return World;
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

    public abstract void Tick();

    public virtual void Update()
    {
        LastPosition = Position;
        LastRotation = Rotation;
        TicksLived += 1;
        
        Metadata.ClearChanges();
    }

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