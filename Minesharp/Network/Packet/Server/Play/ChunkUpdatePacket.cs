using System.Collections;
using DotNetty.Buffers;
using Minesharp.Extension;
using Minesharp.Nbt;
using Minesharp.Utility;

namespace Minesharp.Network.Packet.Server.Play;

public class ChunkUpdatePacket : ServerPacket
{
    public int ChunkX { get; init; }
    public int ChunkY { get; init; }
    public CompoundTag Heightmaps { get; init; }
    public IByteBuffer Data { get; init; }
    public bool TrustEdges { get; init; }
    public BitSet SkyLightMask { get; init; }
    public BitSet BlockLightMask { get; init; }
    public BitSet EmptySkyLightMask { get; init; }
    public BitSet EmptyBlockLightMask { get; init; }
    
    public List<byte[]> SkyLight { get; init; }
    public List<byte[]> BlockLight { get; init; }
}

public class ChunkUpdateCreator : BufferCreator<ChunkUpdatePacket>
{
    public override int PacketId => 0x1F;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override void CreateBuffer(ChunkUpdatePacket packet, IByteBuffer buffer)
    {
        buffer.WriteInt(packet.ChunkX);
        buffer.WriteInt(packet.ChunkY);
        buffer.WriteTag(packet.Heightmaps);
        buffer.WriteVarInt(packet.Data.WriterIndex);
        buffer.WriteBytes(packet.Data);
        
        buffer.WriteVarInt(0);
        
        buffer.WriteBoolean(packet.TrustEdges);
        buffer.WriteBitSet(packet.SkyLightMask);
        buffer.WriteBitSet(packet.BlockLightMask);
        buffer.WriteBitSet(packet.EmptySkyLightMask);
        buffer.WriteBitSet(packet.EmptyBlockLightMask);
        
        buffer.WriteVarInt(packet.SkyLight.Count);
        foreach (var value in packet.SkyLight)
        {
            buffer.WriteVarInt(value.Length);
            buffer.WriteBytes(value);
        }
        
        buffer.WriteVarInt(packet.BlockLight.Count);
        foreach (var value in packet.BlockLight)
        {
            buffer.WriteVarInt(value.Length);
            buffer.WriteBytes(value);
        }
    }
}