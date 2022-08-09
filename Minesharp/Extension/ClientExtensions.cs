using System.Collections;
using DotNetty.Buffers;
using Minesharp.Game;
using Minesharp.Game.Chunks;
using Minesharp.Game.Worlds;
using Minesharp.Nbt;
using Minesharp.Network;
using Minesharp.Network.Packet.Server.Play;
using Minesharp.Utility;

namespace Minesharp.Extension;

public static class ClientExtensions
{
    public static void UpdateChunks(this NetworkClient client)
    {
        var player = client.Player;
        var chunkKeys = new List<ChunkKey>();
        
        var centralX = player.Position.GetBlockX();
        var centralZ = player.Position.GetBlockZ();
        var radius = 12;

        for (var x = centralX - radius; x < centralX + radius; x++)
        {
            for (var z = centralZ - radius; z < centralZ + radius; z++)
            {
                chunkKeys.Add(ChunkKey.Create(x, z));
            }
        }
        
        chunkKeys.Sort((a, b) =>
        {
            var dx = 16 * a.GetX() + 8 - player.Position.X;
            var dz = 16 * a.GetZ() + 8 - player.Position.Z;
            var da = dx * dx + dz * dz;
            
            dx = 16 * b.GetX() + 8 - player.Position.X;
            dz = 16 * b.GetZ() + 8 - player.Position.Z;
            
            var db = dx * dx + dz * dz;
            
            return da.CompareTo(db);
        });

        foreach (var chunkKey in chunkKeys)
        {
            var chunk = player.World.GetChunk(chunkKey);
            if (chunk is null)
            {
                continue;
            }
            
            chunk.Load();

            var buffer = Unpooled.Buffer();
            foreach (var section in chunk.GetSections())
            {
                var data = section.GetData();
                var palette = section.GetPalette();

                buffer.WriteByte(data.GetBitsPerValue());
                if (palette is not null)
                {
                    buffer.WriteVarInt(palette.Count);
                    foreach (var value in palette)
                    {
                        buffer.WriteVarInt(value);
                    }
                }

                var backing = data.GetBacking();
                buffer.WriteVarInt(backing.Length);
                foreach (var value in backing)
                {
                    buffer.WriteLong(value);
                }
                
                buffer.WriteVarInt(1);
                buffer.WriteVarInt(0);
                
                buffer.WriteVarInt(256);
                buffer.WriteBytes(new byte[2048]);
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
            
            client.SendPacket(new ChunkUpdatePacket
            {
                ChunkX = chunk.GetX(),
                ChunkY = chunk.GetZ(),
                Data = buffer,
                Heightmaps = new CompoundTag
                {
                    ["MOTION_BLOCKING"] = new ByteArrayTag(chunk.GetHeightmap())
                },
                TrustEdges = true,
                SkyLightMask = skylightMask,
                BlockLightMask = blockLightMask,
                EmptyBlockLightMask = new BitSet(),
                EmptySkyLightMask = new BitSet(),
                SkyLight = skyLights,
                BlockLight = blockLights
            });
        }
    }
}