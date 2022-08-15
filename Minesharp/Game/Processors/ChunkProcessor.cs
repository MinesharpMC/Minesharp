using Minesharp.Common.Collection;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Nbt;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Processors;

public class ChunkProcessor
{
    private readonly Player player;
    private readonly HashSet<ChunkKey> knownChunks = new();
    private HashSet<ChunkKey> outdatedChunks = new();

    private int previousCentralX;
    private int previousCentralZ;

    public IReadOnlySet<ChunkKey> KnownChunks => knownChunks;
    public IReadOnlySet<ChunkKey> OutdatedChunks => outdatedChunks;

    public ChunkProcessor(Player player)
    {
        this.player = player;
    }

    private void ProcessChunks()
    {
        outdatedChunks = new HashSet<ChunkKey>(knownChunks);

        var world = player.World;
        var position = player.Position;
        
        var centralX = position.BlockX >> 4;
        var centralZ = position.BlockZ >> 4;
        var radius = 5;
        
        var newChunks = new HashSet<ChunkKey>();
        
        for (var x = centralX - radius; x <= centralX + radius; x++)
        for (var z = centralZ - radius; z <= centralZ + radius; z++)
        {
            var key = ChunkKey.Of(x, z);
            if (!knownChunks.Contains(key))
            {
                newChunks.Add(key);
            }
            else
            {
                outdatedChunks.Remove(key);
            }
        }
        
        foreach (var key in newChunks)
        {
            var chunk = world.LoadChunk(key);
            
            chunk.Lock();

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
            
            player.SendPacket(new LoadChunkPacket
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
            
            knownChunks.Add(key);
        }
        
        foreach (var chunkKey in outdatedChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }
            
            player.SendPacket(new UnloadChunkPacket
            {
                ChunkX = chunk.X,
                ChunkZ = chunk.Z
            });

            knownChunks.Remove(chunk.Key);
            chunk.Unlock();
        }

        if (centralX != previousCentralX || centralZ != previousCentralZ)
        {
            player.SendPacket(new SetCenterChunkPacket
            {
                ChunkX = centralX,
                ChunkZ = centralZ
            });
        }

        previousCentralX = centralX;
        previousCentralZ = centralZ;
    }

    private void ProcessBlocks()
    {
        var world = player.World;
        foreach (var chunkKey in knownChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            var blocks = chunk.GetModifiedBlocks();

            foreach (var block in blocks)
            {
                player.SendPacket(new BlockChangePacket
                {
                    Position = block.Position,
                    Type = block.Type
                });
            }
        }
    }

    public void Tick()
    {
        ProcessChunks();
        ProcessBlocks();
    }
}