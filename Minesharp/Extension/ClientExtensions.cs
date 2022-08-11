using DotNetty.Buffers;
using Minesharp.Game.Chunks;
using Minesharp.Nbt;
using Minesharp.Network;
using Minesharp.Network.Packet.Server.Play;
using Minesharp.Utility;

namespace Minesharp.Extension;

public static class ClientExtensions
{
    public static void UpdateChunks(this NetworkSession session)
    {
        var player = session.Player;
        var chunkKeys = new List<ChunkKey>();
        
        var centralX = player.Position.BlockX;
        var centralZ = player.Position.BlockZ;
        var radius = 3;

        for (var x = centralX - radius; x <= centralX + radius; x++)
        {
            for (var z = centralZ - radius; z <= centralZ + radius; z++)
            {
                chunkKeys.Add(ChunkKey.Create(x, z));
            }
        }

        foreach (var chunkKey in chunkKeys)
        {
            var chunk = player.World.LoadChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }

            var buffer = Unpooled.Buffer();
            foreach (var section in chunk.Sections)
            {
                if (section is null)
                {
                    continue;
                }

                buffer.WriteShort(section.BlockCount);

                buffer.WriteByte(section.State.BitsPerEntry);
                if (section.State.HasPalette)
                {
                    buffer.WriteVarInt(section.State.Palette.Count);
                    foreach (var value in section.State.Palette)
                    {
                        buffer.WriteVarInt(value);
                    }
                }

                buffer.WriteVarInt(section.State.Storage.Count);
                foreach (var value in section.State.Storage.Values)
                {
                    buffer.WriteLong(value);
                }
            }

            for (var i = 0; i < 256; i++)
            {
                buffer.WriteInt(0);
            }

            var skylightMask = new BitSet();
            var blockLightMask = new BitSet();
            for (var i = 0; i < Chunk.SectionCount + 2; i++)
            {
                skylightMask.Set(i);
                blockLightMask.Set(i);
            }

            var skyLights = new List<byte[]>();
            var blockLights = new List<byte[]>();
            for (var i = 0; i < 18; i++)
            {
                skyLights.Add(Chunk.EmptyLight);
                blockLights.Add(Chunk.EmptyLight);
            }

            session.SendPacket(new ChunkUpdatePacket
            {
                ChunkX = chunk.X,
                ChunkZ = chunk.Z,
                Data = buffer,
                Heightmaps = new CompoundTag
                {
                    ["MOTION_BLOCKING"] = new ByteArrayTag(chunk.Heightmap)
                },
                TrustEdges = true,
                EmptyBlockLightMask = new BitSet(),
                EmptySkyLightMask = new BitSet(),
                SkyLight = skyLights,
                BlockLight = blockLights,
                SkyLightMask = skylightMask,
                BlockLightMask = blockLightMask
            });
        }
    }
}