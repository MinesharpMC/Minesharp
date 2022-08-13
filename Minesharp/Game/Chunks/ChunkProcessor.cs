using Minesharp.Game.Entities;
using Minesharp.Nbt;
using Minesharp.Packet.Common;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Game.Chunks;

public class ChunkProcessor
{
    private readonly HashSet<ChunkKey> knownChunks = new();
    private readonly Player player;

    private bool firstStream = true;
    
    private int previousCentralX;
    private int previousCentralZ;
    private int previousRadius;

    public ChunkProcessor(Player player)
    {
        this.player = player;
    }

    private void ProcessChunks()
    {
        var world = player.World;
        var newChunks = new List<ChunkKey>();
        var previousChunks = new HashSet<ChunkKey>();

        var position = player.Position.ToBlockPosition();
        
        var centralX = position.X >> 4;
        var centralZ = position.Z >> 4;
        var radius = 1; // TODO : View distance

        if (firstStream)
        {
            firstStream = false;
            for (var x = centralX - radius; x <= centralX + radius; x++)
            for (var z = centralZ - radius; z <= centralZ + radius; z++)
            {
                newChunks.Add(ChunkKey.Of(x, z));
            }
        }
        else if (Math.Abs(centralX - previousCentralX) > radius || Math.Abs(centralZ - previousCentralZ) > radius)
        {
            // knownChunks.Clear();
            for (var x = centralX - radius; x <= centralX + radius; x++)
            for (var z = centralZ - radius; z <= centralZ + radius; z++)
            {
                newChunks.Add(ChunkKey.Of(x, z));
            }
        }
        else if (centralX != previousCentralX || centralZ != previousCentralZ || radius != previousRadius)
        {
            previousChunks = new HashSet<ChunkKey>(knownChunks);
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
                    previousChunks.Remove(key);
                }
            }
        }
        else
        {
            return;
        }
        
        previousCentralX = centralX;
        previousCentralZ = centralZ;
        previousRadius = radius;

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

        if (newChunks.Any())
        {
            Log.Information("Updating chunks");
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
                for (var i = 0; i < ChunkConstants.SectionCount + 2; i++)
                {
                    mask.Set(i);
                }

                var lights = new List<byte[]>();
                for (var i = 0; i < 18; i++)
                {
                    lights.Add(ChunkConstants.EmptyLight);
                }

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
        }

        if (previousChunks.Any())
        {
            foreach (var chunkKey in previousChunks)
            {
                player.SendPacket(new UnloadChunkPacket
                {
                    ChunkX = chunkKey.X,
                    ChunkZ = chunkKey.Z
                });

                knownChunks.Remove(chunkKey);
            }

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