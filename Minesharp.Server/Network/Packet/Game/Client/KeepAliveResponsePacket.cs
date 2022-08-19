using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Client;

public sealed class KeepAliveResponsePacket : GamePacket
{
    public long Timestamp { get; init; }
}

public sealed class KeepAliveResponsePacketCodec : GamePacketCodec<KeepAliveResponsePacket>
{
    public override int PacketId => 0x11;

    protected override KeepAliveResponsePacket Decode(IByteBuffer buffer)
    {
        var timestamp = buffer.ReadLong();

        return new KeepAliveResponsePacket
        {
            Timestamp = timestamp
        };
    }

    protected override void Encode(KeepAliveResponsePacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Timestamp);
    }
}