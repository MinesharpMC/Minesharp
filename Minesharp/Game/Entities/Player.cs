using Minesharp.Common.Enum;
using Minesharp.Common.Extension;
using Minesharp.Common.Meta;
using Minesharp.Game.Blocks;
using Minesharp.Game.Entities.Meta;
using Minesharp.Game.Inventories;
using Minesharp.Game.Modules;
using Minesharp.Network;
using Minesharp.Packet;
using Minesharp.Packet.Game;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Entities;

public sealed class Player : LivingEntity
{
    private readonly NetworkSession session;
    
    private readonly ChunkModule chunkModule;
    private readonly EntityModule entityModule;
    private readonly HealthModule healthModule;
    private readonly BreakModule breakModule;
    private readonly MetadataModule metadataModule;

    public Player(NetworkSession session)
    {
        this.session = session;

        chunkModule = new ChunkModule(this);
        entityModule = new EntityModule(this, chunkModule);
        healthModule = new HealthModule(this);
        breakModule = new BreakModule(this);
        metadataModule = new MetadataModule(this);
    }

    public string Username { get; set; }
    public GameMode GameMode { get; set; }
    public string Locale { get; set; }
    public byte ViewDistance { get; set; }
    public Hand MainHand { get; set; }
    public int Food { get; set; }
    public float Exhaustion { get; set; }
    public float Saturation { get; set; }
    public PlayerInventory Inventory { get; set; }

    public Block Breaking
    {
        get => breakModule.Block;
        set => breakModule.Block = value;
    }

    public bool IsSprinting
    {
        get => Metadata.GetBoolean(MetadataIndex.Status, StatusFlags.Sprinting);
        set => Metadata.SetBoolean(MetadataIndex.Status, StatusFlags.Sprinting, value);
    }

    public bool IsSneaking
    {
        get => Metadata.GetBoolean(MetadataIndex.Status, StatusFlags.Sneaking);
        set => Metadata.SetBoolean(MetadataIndex.Status, StatusFlags.Sneaking, value);
    }

    public bool CanSee(Entity entity)
    {
        if (entity == this)
        {
            return false;
        }
        
        return entityModule.HasLoaded(entity);
    }

    public bool CanSee(Block block)
    {
        var chunk = World.GetChunkAt(block.Position);
        if (chunk is null)
        {
            return false;
        }

        return chunkModule.HasLoaded(chunk);
    }

    public void RefreshChunks()
    {
        chunkModule.Tick();
    }

    public override void Tick()
    {
        chunkModule.Tick();
        entityModule.Tick();
        healthModule.Tick();
        breakModule.Tick();
        metadataModule.Tick();

        TicksLived += 1;
    }

    public override void Update()
    {
        chunkModule.Update();
        entityModule.Update();
        healthModule.Update();
        breakModule.Update();
        metadataModule.Update();

        LastPosition = Position;
        LastRotation = Rotation;
    }

    public override IEnumerable<GamePacket> GetSpawnPackets()
    {
        return new GamePacket[]
        {
            new SpawnPlayerPacket(Id, UniqueId, Position, Rotation),
            new EntityMetadataPacket(Id, Metadata.GetEntries()),
            new HeadRotationPacket(Id, Rotation.GetIntYaw()),
            new EquipmentPacket(Id, EquipmentSlot.MainHand, Inventory.ItemInMainHand)
        };
    }

    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }
}