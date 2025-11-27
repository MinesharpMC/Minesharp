using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Status.Client;

public sealed class StatusRequestPacket : StatusPacket
{
}

public sealed class StatusRequestPacketCodec : StatusPacketDecoder<StatusRequestPacket>
{
    public override int PacketId => 0x00;

    protected override StatusRequestPacket Decode(IByteBuffer buffer)
    {
        return new StatusRequestPacket();
    }
}