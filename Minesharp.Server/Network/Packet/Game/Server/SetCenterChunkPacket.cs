using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class SetCenterChunkPacket : GamePacket
{
    public SetCenterChunkPacket()
    {
    }

    public SetCenterChunkPacket(int chunkX, int chunkZ)
    {
        ChunkX = chunkX;
        ChunkZ = chunkZ;
    }

    public int ChunkX { get; init; }
    public int ChunkZ { get; init; }
}

public class SetCenterChunkPacketCodec : GamePacketEncoder<SetCenterChunkPacket>
{
    public override int PacketId => 0x48;

    protected override void Encode(SetCenterChunkPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.ChunkX);
        buffer.WriteVarInt(packet.ChunkZ);
    }
}