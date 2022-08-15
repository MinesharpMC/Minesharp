using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Chunks;
using Minesharp.Game.Processors;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Packet;

namespace Minesharp.Game.Entities;

public sealed class Player : Entity, IEquatable<Player>
{
    private readonly NetworkSession session;
    private readonly ChunkProcessor chunkProcessor;
    private readonly EntityProcessor entityProcessor;

    public IReadOnlySet<ChunkKey> KnownChunks => chunkProcessor.KnownChunks;
    public IReadOnlySet<ChunkKey> OutdatedChunks => chunkProcessor.OutdatedChunks;
    public IReadOnlySet<int> KnownEntities => entityProcessor.KnownEntities;

    public Player(NetworkSession session) : base(EntityType.Player)
    {
        this.session = session;
        this.chunkProcessor = new ChunkProcessor(this);
        this.entityProcessor = new EntityProcessor(this);
    }
    
    public string Username { get; set; }
    public Server Server { get; init; }
    public GameMode GameMode { get; set; }
    public string Locale { get; set; }
    public byte ViewDistance { get; set; }
    public Hand MainHand { get; set; }

    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }

    public bool Known(Entity entity)
    {
        return KnownEntities.Contains(entity.Id);
    }

    public override void Tick()
    {
        chunkProcessor.Tick();
        entityProcessor.Tick();
    }

    public bool Equals(Player other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return ReferenceEquals(this, other) || UniqueId.Equals(other.UniqueId);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Player other && Equals(other);
    }

    public override int GetHashCode()
    {
        return UniqueId.GetHashCode();
    }

    public static bool operator ==(Player left, Player right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Player left, Player right)
    {
        return !Equals(left, right);
    }
}