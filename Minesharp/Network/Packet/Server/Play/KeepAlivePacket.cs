using DotNetty.Buffers;

namespace Minesharp.Network.Packet.Server.Play;

public class KeepAlivePacket : ServerPacket
{
    public long Timestamp { get; init; }
}

public class KeepAliveCreator : BufferCreator<KeepAlivePacket>
{
    public override int PacketId => 0x1E;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override void CreateBuffer(KeepAlivePacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Timestamp);
    }
}