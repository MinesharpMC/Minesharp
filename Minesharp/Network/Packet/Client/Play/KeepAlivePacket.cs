using DotNetty.Buffers;

namespace Minesharp.Network.Packet.Client.Play;

public class KeepAlivePacket : ClientPacket
{
    public long Timestamp { get; init; }
}

public class KeepAliveCreator : PacketCreator<KeepAlivePacket>
{
    public override int PacketId => 0x11;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override KeepAlivePacket CreatePacket(IByteBuffer buffer)
    {
        var timestamp = buffer.ReadLong();

        return new KeepAlivePacket
        {
            Timestamp = timestamp
        };
    }
}