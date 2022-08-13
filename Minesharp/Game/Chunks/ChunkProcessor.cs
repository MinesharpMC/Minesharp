using Minesharp.Game.Entities;
using Minesharp.Nbt;
using Minesharp.Packet.Common;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Chunks;

public class ChunkProcessor
{
    private readonly HashSet<ChunkKey> knownChunks = new();
    private readonly Player player;

    private bool firstStream = true;
    private BlockPosition previousPosition;

    public ChunkProcessor(Player player)
    {
        this.player = player;
    }

    private void ProcessChunks()
    {
        var world = player.World;
        var newChunks = new List<ChunkKey>();
        var previousChunks = new HashSet<ChunkKey>();

        var position = player.Position;
        var blockPosition = position.ToBlockPosition();
        var radius = 1; // TODO : View distance

        if (firstStream)
        {
            firstStream = false;
            for (var x = blockPosition.X - radius; x <= blockPosition.X + radius; x++)
            for (var z = blockPosition.Z - radius; z <= blockPosition.Z + radius; z++)
                newChunks.Add(ChunkKey.Create(x, z));
        }
        else if (Math.Abs(blockPosition.X - previousPosition.X) > radius ||
                 Math.Abs(blockPosition.Z - previousPosition.Z) > radius)
        {
            knownChunks.Clear();
            for (var x = blockPosition.X - radius; x <= blockPosition.X + radius; x++)
            for (var z = blockPosition.Z - radius; z <= blockPosition.Z + radius; z++)
                newChunks.Add(ChunkKey.Create(x, z));
        }
        else if (previousPosition.X != blockPosition.X || previousPosition.Z != blockPosition.Z)
        {
            previousChunks = new HashSet<ChunkKey>(knownChunks);
            for (var x = blockPosition.X - radius; x <= blockPosition.X + radius; x++)
            for (var z = blockPosition.Z - radius; z <= blockPosition.Z + radius; z++)
            {
                var key = ChunkKey.Create(x, z);
                if (knownChunks.Contains(key))
                    previousChunks.Remove(key);
                else
                    newChunks.Add(key);
            }
        }
        else
        {
            return;
        }

        previousPosition = blockPosition;

        newChunks.Sort((a, b) =>
        {
            var dx = 16 * a.X + 8 - position.X;
            var dz = 16 * a.Z + 8 - position.Z;

            var da = dx * dx + dz * dz;
            dx = 16 * b.X + 8 - position.X;
            dz = 16 * b.X + 8 - position.Z;

            var db = dx * dx + dz * dz;

            return da.CompareTo(db);
        });

        foreach (var key in newChunks)
        {
            var chunk = world.GetChunk(key);

            var sections = chunk.Sections
                .Where(x => x is not null)
                .Select(x => new SectionInfo
                {
                    Bits = x.Bits,
                    BlockCount = (short)x.BlockCount,
                    UsePalette = x.UsePalette,
                    Mapping = x.Mapping,
                    Palette = x.Palette
                });

            var biomes = Enumerable.Range(0, 256)
                .Select(_ => 0);

            var mask = new BitSet();
            for (var i = 0; i < ChunkConstants.SectionCount + 2; i++) mask.Set(i);

            var lights = new List<byte[]>();
            for (var i = 0; i < 18; i++) lights.Add(ChunkConstants.EmptyLight);

            knownChunks.Add(key);

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
        }

        if (previousChunks.Any())
        {
            foreach (var chunkKey in previousChunks)
                knownChunks.Remove(chunkKey);
            
            previousChunks.Clear();
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