using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class SetCenterChunkPacket : GamePacket
{
    public int ChunkX { get; init; }
    public int ChunkZ { get; init; }

    public SetCenterChunkPacket()
    {
    }

    public SetCenterChunkPacket(int chunkX, int chunkZ)
    {
        ChunkX = chunkX;
        ChunkZ = chunkZ;
    }
}

public class SetCenterChunkPacketCodec : GamePacketCodec<SetCenterChunkPacket>
{
    public override int PacketId => 0x48;

    protected override SetCenterChunkPacket Decode(IByteBuffer buffer)
    {
        var chunkX = buffer.ReadVarInt();
        var chunkZ = buffer.ReadVarInt();

        return new SetCenterChunkPacket
        {
            ChunkX = chunkX,
            ChunkZ = chunkZ
        };
    }

    protected override void Encode(SetCenterChunkPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.ChunkX);
        buffer.WriteVarInt(packet.ChunkZ);
    }
}