using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Status.Client;

public sealed class StatusRequestPacket : StatusPacket
{
}

public sealed class StatusRequestPacketCodec : StatusPacketCodec<StatusRequestPacket>
{
    public override int PacketId => 0x00;

    protected override StatusRequestPacket Decode(IByteBuffer buffer)
    {
        return new StatusRequestPacket();
    }

    protected override void Encode(StatusRequestPacket packet, IByteBuffer buffer)
    {
        // Something to write into buffer
    }
}