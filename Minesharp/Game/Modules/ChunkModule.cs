using Minesharp.Common.Collection;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Nbt;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Modules;

public class ChunkModule
{
    private readonly Player player;
    private readonly HashSet<ChunkKey> chunks = new();

    public ChunkModule(Player player)
    {
        this.player = player;
    }

    public void Tick()
    {
        var world = player.World;
        var position = player.Position;
        var lastPosition = player.LastPosition;

        var centralX = position.BlockX >> 4;
        var centralZ = position.BlockZ >> 4;
        var radius = player.ViewDistance + 1;

        var newChunks = new HashSet<ChunkKey>();
        var outdatedChunks = new HashSet<ChunkKey>(chunks);

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

        foreach (var chunkKey in outdatedChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }

            player.SendPacket(new UnloadChunkPacket(chunk.X, chunk.Z));

            chunks.Remove(chunk.Key);
            chunk.RemoveLock();
        }

        foreach (var key in newChunks)
        {
            var chunk = world.LoadChunk(key);

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

            chunks.Add(key);
        }

        if (position.BlockX != lastPosition.BlockX || position.BlockZ != lastPosition.BlockZ)
        {
            var chunk = world.GetChunkAt(position);
            var previousChunk = world.GetChunkAt(lastPosition);

            if (chunk != previousChunk)
            {
                player.SendPacket(new SetCenterChunkPacket(chunk.X, chunk.Z));
            }
        }
    }

    public void Update()
    {
        var world = player.World;
        foreach (var chunkKey in chunks)
        {
            var chunk = world.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }

            var blocks = chunk.GetModifiedBlocks();
            foreach (var block in blocks)
            {
                player.SendPacket(new BlockChangePacket(block.Position, block.Type));
            }
        }
    }

    public bool HasLoaded(Chunk chunk)
    {
        return chunks.Contains(chunk.Key);
    }
}