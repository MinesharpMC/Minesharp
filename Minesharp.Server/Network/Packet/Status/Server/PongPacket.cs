using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Status.Server;

public sealed class PongPacket : StatusPacket
{
    /// <summary>
    ///     Should be the same as sent by the client.
    /// </summary>
    public long Payload { get; init; }
}

public sealed class PongPacketCodec : StatusPacketCodec<PongPacket>
{
    public override int PacketId => 0x01;

    protected override PongPacket Decode(IByteBuffer buffer)
    {
        var payload = buffer.ReadLong();

        return new PongPacket
        {
            Payload = payload
        };
    }

    protected override void Encode(PongPacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Payload);
    }
}