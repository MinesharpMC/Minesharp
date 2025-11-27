using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Status.Server;

public sealed class PongPacket : StatusPacket
{
    /// <summary>
    ///     Should be the same as sent by the client.
    /// </summary>
    public long Payload { get; init; }
}

public sealed class PongPacketCodec : StatusPacketEncoder<PongPacket>
{
    public override int PacketId => 0x01;

    protected override void Encode(PongPacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Payload);
    }
}