using Minesharp.Common.Collection;
using Minesharp.Game.Entities;
using Minesharp.Nbt;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Game.Chunks;

public class ChunkProcessor
{
    private readonly Player player;
    private readonly HashSet<ChunkKey> knownChunks = new();

    public ChunkProcessor(Player player)
    {
        this.player = player;
    }

    private void ProcessChunks()
    {
        var world = player.World;
        var newChunks = new List<ChunkKey>();

        var position = player.Position;
        
        var centralX = position.BlockX >> 4;
        var centralZ = position.BlockZ >> 4;
        var radius = 5;

        for (var x = centralX - radius; x <= centralX + radius; x++)
        for (var z = centralZ - radius; z <= centralZ + radius; z++)
        {
            var key = ChunkKey.Of(x, z);
            if (!knownChunks.Contains(key))
            {
                newChunks.Add(ChunkKey.Of(x, z));
            }
        }

        foreach (var key in newChunks)
        {
            var chunk = world.GetChunk(key);

            var sections = chunk.Sections
                .Where(x => x is not null)
                .Select(x => new SectionInfo
                {
                    Bits = x.Bits,
                    BlockCount = (short)x.BlockCount,
                    Mapping = x.Mapping,
                    Palette = x.Palette
                });

            var biomes = Enumerable.Range(0, 256)
                .Select(_ => 0);

            var mask = new BitSet();
            for (var i = 0; i < ChunkConstants.SectionCount + 2; i++)
            {
                mask.Set(i);
            }

            var lights = new List<byte[]>();
            for (var i = 0; i < 18; i++)
            {
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
    }

    private void ProcessBlocks()
    {
        var world = player.World;
        foreach (var chunkKey in knownChunks)
        {
            // TODO : Update modified blocks
        }
    }

    public void Tick()
    {
        ProcessChunks();
        ProcessBlocks();
    }
}