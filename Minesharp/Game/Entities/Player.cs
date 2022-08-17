using Minesharp.Common.Collection;
using Minesharp.Common.Enum;
using Minesharp.Common.Extension;
using Minesharp.Common.Meta;
using Minesharp.Extension;
using Minesharp.Game.Blocks;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities.Meta;
using Minesharp.Nbt;
using Minesharp.Network;
using Minesharp.Packet;
using Minesharp.Packet.Game;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Entities;

public sealed class Player : LivingEntity
{
    private readonly NetworkSession session;
    private readonly HashSet<ChunkKey> chunks = new();
    private readonly HashSet<int> entities = new();

    private Block digging;
    private long diggingTicks;
    private long diggingTicksRequired;

    public Player(NetworkSession session)
    {
        this.session = session;
    }

    public string Username { get; set; }
    public GameMode GameMode { get; set; }
    public string Locale { get; set; }
    public byte ViewDistance { get; set; }
    public Hand MainHand { get; set; }
    public int Food { get; set; }
    public float Exhaustion { get; set; }
    public float Saturation { get; set; }

    public Block Digging
    {
        get => digging;
        set
        {
            if (digging == value)
            {
                return;
            }
            
            if (value is null) // When set to null we stop breaking this block
            {
                digging.ResetBreakStage(this);
                digging = value;
                return;
            }

            digging = value;
            diggingTicks = 0L;
            diggingTicksRequired = (long)((1.5 * digging.Type.GetHardness() * Server.TickRate) + 0.5);
            
            digging.ShowBreakStage(0, this);
        }
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
        
        var chunk = World.GetChunkAt(entity.Position);
        if (chunk is null)
        {
            return false;
        }

        return chunks.Contains(chunk.Key);
    }

    public bool CanSee(Chunk chunk)
    {
        return chunks.Contains(chunk.Key);
    }

    public bool CanSee(Block block)
    {
        var chunk = World.GetChunkAt(block.Position);
        if (chunk is null)
        {
            return false;
        }

        return chunks.Contains(chunk.Key);
    }

    private void UpdateChunks()
    {
        var centralX = Position.BlockX >> 4;
        var centralZ = Position.BlockZ >> 4;
        var radius = ViewDistance + 1;
        
        var newChunks = new HashSet<ChunkKey>();
        var outdatedChunks = new HashSet<ChunkKey>(chunks);
        
        // Filter new chunks
        for (var x = centralX - radius; x <= centralX + radius; x++)
        for (var z = centralZ - radius; z <= centralZ + radius; z++)
        {
            var key = ChunkKey.Of(x, z);
            if (!chunks.Contains(key))
            {
                newChunks.Add(key);
            }
            else
            {
                outdatedChunks.Remove(key);
            }
        }
        
        // Unload outdated chunks
        foreach (var chunkKey in outdatedChunks)
        {
            var chunk = World.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }
            
            SendPacket(new UnloadChunkPacket(chunk.X, chunk.Z));

            chunks.Remove(chunk.Key);
            chunk.RemoveLock();
        }

        // Load new chunks
        foreach (var key in newChunks)
        {
            var chunk = World.LoadChunk(key);
            
            chunk.AddLock();

            var sections = chunk.Sections
                .OrderBy(x => x.Key)
                .Select(x => new SectionInfo
                {
                    Bits = x.Value.Bits,
                    BlockCount = (short)x.Value.BlockCount,
                    Mapping = x.Value.Mapping,
                    Palette = x.Value.Palette
                });

            var biomes = Enumerable.Range(0, 256)
                .Select(_ => 0);
            
            var mask = new BitSet();
            var lights = new List<byte[]>();
            for (var i = 0; i < 18; i++)
            {
                mask.Set(i);
                lights.Add(ChunkConstants.EmptyLight);
            }
            
            SendPacket(new LoadChunkPacket
            {
                ChunkX = chunk.X,
                ChunkZ = chunk.Z,
                ChunkInfo = new ChunkInfo
                {
                    Sections = sections,
                    Biomes = biomes
                },
                Heightmaps = new CompoundTag
                {
                    ["MOTION_BLOCKING"] = new ByteArrayTag(chunk.Heightmap)
                },
                TrustEdges = true,
                EmptyBlockLightMask = new BitSet(),
                EmptySkyLightMask = new BitSet(),
                SkyLight = lights,
                BlockLight = lights,
                SkyLightMask = mask,
                BlockLightMask = mask
            });
            
            chunks.Add(key);
        }

        // Update center chunk
        if (Position.BlockX != LastPosition.BlockX || Position.BlockZ != LastPosition.BlockZ)
        {
            var chunk = World.GetChunkAt(Position);
            var previousChunk = World.GetChunkAt(LastPosition);

            if (chunk != previousChunk)
            {
                SendPacket(new SetCenterChunkPacket(chunk.X, chunk.Z));
            }
        }
    }
    
    private void UpdateHealth()
    {
        if (Exhaustion > 4.0f)
        {
            Exhaustion -= 4.0f;
            if (Saturation > 0f)
            {
                Saturation = Math.Max(Saturation - 1f, 0f);
                SendHealth();
            }
            else if (World.Difficulty is not Difficulty.Peaceful)
            {
                Food = Math.Max(Food - 1, 0);
                SendHealth();
            }
        }

        if (Health < MaximumHealth)
        {
            if ((Food >= 18 && TicksLived % 80 == 0) || World.Difficulty == Difficulty.Peaceful)
            {
                Exhaustion = Math.Min(Exhaustion + 3.0f, 40.0f);
                Saturation -= 3;

                Health = Math.Min(MaximumHealth, Health + 1);
                SendHealth();
            }
        }

        switch (World.Difficulty)
        {
            case Difficulty.Peaceful:
                if (Food < 20 && TicksLived % 20 == 0)
                {
                    Food++;
                }
                break;
            case Difficulty.Easy:
                if (Food == 0 && Health > 10 && TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }
                break;
            case Difficulty.Normal:
                if (Food == 0 && Health > 1 && TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }
                break;
            case Difficulty.Hard:
                if (Food == 0 && TicksLived % 80 == 0)
                {
                    // TODO : Damage
                }
                break;
        }
    }

    private void UpdateMetadata()
    {
        var changes = Metadata.GetChanges();
        if (changes.Any())
        {
            session.SendPacket(new EntityMetadataPacket(Id, changes));
        }
    }

    private void UpdateDigging()
    {
        if (Digging == null)
        {
            return;
        }
        
        if (++diggingTicks <= diggingTicksRequired)
        {
            Digging.ShowBreakStage((byte)(10.0 * (diggingTicks - 1) / diggingTicksRequired), this);
            return;
        }

        Exhaustion = Math.Min(Exhaustion + 0.005f, 40f);
        
        Digging.ResetBreakStage(this);
        Digging.Break(this);
        
        Digging = null;
    }
    
    private void UpdateEntities()
    {
        var removedEntities = new HashSet<int>(entities);
        foreach (var entity in World.GetEntities())
        {
            var canSee = CanSee(entity);
            if (canSee)
            {
                if (!entities.Contains(entity.Id))
                {
                    var packets = entity.GetSpawnPackets();
                    foreach (var packet in packets)
                    {
                        session.SendPacket(packet);
                    }

                    entities.Add(entity.Id);
                }
                else
                {
                    var packets = entity.GetUpdatePackets();
                    foreach (var packet in packets)
                    {
                        session.SendPacket(packet);
                    }
                    
                    removedEntities.Remove(entity.Id);
                }
            }
        }

        if (removedEntities.Any())
        {
            session.SendPacket(new RemoveEntitiesPacket(removedEntities.ToList()));
            foreach (var removedEntity in removedEntities)
            {
                entities.Remove(removedEntity);
            }
        }
    }

    private void UpdateBlocks()
    {
        foreach (var chunkKey in chunks)
        {
            var chunk = World.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }
            
            var modifiedBlocks = chunk.GetModifiedBlocks();
            foreach (var modifiedBlock in modifiedBlocks)
            {
                session.SendPacket(new BlockChangePacket(modifiedBlock.Position, modifiedBlock.Type));
            }
        }
    }

    public override void Tick()
    {
        UpdateChunks();
        UpdateEntities();
        UpdateDigging();
        UpdateHealth();

        TicksLived += 1;
    }
    
    public override void Update()
    {
        UpdateBlocks();
        UpdateMetadata();
        
        LastPosition = Position;
        LastRotation = Rotation;
        
        Metadata.ClearChanges();
    }

    public override IEnumerable<GamePacket> GetSpawnPackets()
    {
        return new GamePacket[]
        {
            new SpawnPlayerPacket(Id, UniqueId, Position, Rotation),
            new EntityMetadataPacket(Id, Metadata.GetEntries()),
            new HeadRotationPacket(Id, Rotation.GetIntYaw())
        };
    }
    
    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }
    
    public void SendHealth()
    {
        var health = (float) (Health / MaximumHealth * 20);
        SendPacket(new HealthPacket(health, Food, Saturation));
    }

    public void SendPosition()
    {
        SendPacket(new SyncPositionPacket(Position, Rotation));
    }
}