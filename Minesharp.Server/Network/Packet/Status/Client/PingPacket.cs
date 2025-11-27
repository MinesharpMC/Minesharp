using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Status.Client;

public sealed class PingPacket : StatusPacket
{
    /// <summary>
    ///     May be any number.
    ///     Notchian clients use a system-dependent time value which is counted in milliseconds.
    /// </summary>
    public long Payload { get; init; }
}

public sealed class PingPacketCodec : StatusPacketDecoder<PingPacket>
{
    public override int PacketId => 0x01;

    protected override PingPacket Decode(IByteBuffer buffer)
    {
        var payload = buffer.ReadLong();

        return new PingPacket
        {
            Payload = payload
        };
    }
}