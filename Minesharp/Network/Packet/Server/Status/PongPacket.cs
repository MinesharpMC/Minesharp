using DotNetty.Buffers;
using Minesharp.Network.Common;

namespace Minesharp.Network.Packet.Server.Status;

public class PongPacket : ServerPacket
{
    public long Payload { get; init; }

    public override string ToString()
    {
        return $"{nameof(Payload)}: {Payload}";
    }
}

public class PongCreator : BufferCreator<PongPacket>
{
    public override int PacketId => 0x01;
    public override NetworkProtocol Protocol => NetworkProtocol.Status;

    protected override void CreateBuffer(PongPacket packet, IByteBuffer buffer)
    {
        buffer.WriteLong(packet.Payload);
    }
}