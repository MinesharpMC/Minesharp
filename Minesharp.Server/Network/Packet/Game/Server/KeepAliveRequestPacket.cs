using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class KeepAliveRequestPacket : GamePacket
{
    public long Timestamp { get; init; }
}

public sealed class KeepAliveRequestPacketCodec : GamePacketCodec<KeepAliveRequestPacket>
{
    public override int PacketId => 0x1E;

    protected override KeepAliveRequestPacket Decode(IByteBuffer buffer)
    {
        var timestamp = buffer.ReadLong();

        return new KeepAliveRequestPacket
        {
            Timestamp = timestamp
        };
    }

    protected override void Encode(KeepAliveRequestPacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Timestamp);
    }
}