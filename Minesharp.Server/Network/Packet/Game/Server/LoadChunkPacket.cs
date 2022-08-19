using DotNetty.Buffers;
using Minesharp.Nbt;
using Minesharp.Server.Extension;
using Minesharp.Server.Utility;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class LoadChunkPacket : GamePacket
{
    public int ChunkX { get; init; }
    public int ChunkZ { get; init; }
    public CompoundTag Heightmaps { get; init; }
    public ChunkInfo ChunkInfo { get; init; }
    public bool TrustEdges { get; init; }
    public BitSet SkyLightMask { get; init; }
    public BitSet BlockLightMask { get; init; }
    public BitSet EmptySkyLightMask { get; init; }
    public BitSet EmptyBlockLightMask { get; init; }
    public List<byte[]> SkyLight { get; init; }
    public List<byte[]> BlockLight { get; init; }
}

public sealed class ChunkInfo
{
    public IEnumerable<SectionInfo> Sections { get; init; }
    public IEnumerable<int> Biomes { get; init; }
}

public sealed class SectionInfo
{
    public int BlockCount { get; init; }
    public byte Bits { get; init; }
    public IEnumerable<int> Palette { get; init; }
    public IEnumerable<long> Mapping { get; init; }
}

public sealed class LoadChunkPacketCodec : GamePacketCodec<LoadChunkPacket>
{
    public override int PacketId => 0x1F;

    protected override LoadChunkPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException("This packet decoding is not yet supported");
    }

    protected override void Encode(LoadChunkPacket packet, IByteBuffer buffer)
    {
        buffer.WriteInt(packet.ChunkX);
        buffer.WriteInt(packet.ChunkZ);
        buffer.WriteTag(packet.Heightmaps);
        buffer.WriteChunkInfo(packet.ChunkInfo);

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