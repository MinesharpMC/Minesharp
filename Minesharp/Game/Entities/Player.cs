using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
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
    private readonly DiggingProcessor diggingProcessor;

    public Player(NetworkSession session) : base(EntityType.Player)
    {
        this.session = session;
        this.chunkProcessor = new ChunkProcessor(this);
        this.entityProcessor = new EntityProcessor(this);
        this.diggingProcessor = new DiggingProcessor(this);
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

    public void StartDigging(Block block)
    {
        diggingProcessor.Start(block);
    }

    public void StopDigging()
    {
        diggingProcessor.Stop();
    }

    public bool CanSee(Entity entity)
    {
        if (entity == this)
        {
            return false;
        }
        
        var chunk = World.GetChunkAt(entity.Position);
        if (chunk is null)
        {
            return false;
        }

        return chunkProcessor.HasLoaded(chunk.Key);
    }

    public bool CanSee(Block block)
    {
        var chunk = World.GetChunkAt(block.Position);
        if (chunk is null)
        {
            return false;
        }

        return chunkProcessor.HasLoaded(chunk.Key);
    }
    

    public override void Tick()
    {
        diggingProcessor.Tick();
        chunkProcessor.Tick();
        entityProcessor.Tick();
    }

    public override void LateTick()
    {
        chunkProcessor.LateTick();
        
        LastPosition = Position;
        LastRotation = Rotation;
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