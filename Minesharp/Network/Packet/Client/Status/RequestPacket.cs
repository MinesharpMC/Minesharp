using DotNetty.Buffers;
using Minesharp.Network.Common;

namespace Minesharp.Network.Packet.Client.Status;

public class RequestPacket : ClientPacket
{
    
}

public class RequestCreator : PacketCreator<RequestPacket>
{
    public override int PacketId => 0x0;
    public override NetworkProtocol Protocol => NetworkProtocol.Status;

    protected override RequestPacket CreatePacket(IByteBuffer buffer)
    {
        return new RequestPacket();
    }
}