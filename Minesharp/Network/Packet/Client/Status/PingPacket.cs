using DotNetty.Buffers;
using Minesharp.Network.Common;

namespace Minesharp.Network.Packet.Client.Status;

public class PingPacket : ClientPacket
{
    public long Payload { get; init; }

    public override string ToString()
    {
        return $"{nameof(Payload)}: {Payload}";
    }
}

public class PingCreator : PacketCreator<PingPacket>
{
    public override int PacketId => 0x01;
    public override NetworkProtocol NetworkProtocol => NetworkProtocol.Status;

    protected override PingPacket CreatePacket(IByteBuffer buffer)
    {
        var payload = buffer.ReadLong();
        
        return new PingPacket
        {
            Payload = payload
        };
    }
}