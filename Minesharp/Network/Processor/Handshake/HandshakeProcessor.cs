using Minesharp.Network.Packet.Client.Handshake;

namespace Minesharp.Network.Processor.Handshake;

public class HandshakeProcessor : PacketProcessor<HandshakePacket>
{
    protected override void Process(NetworkSession session, HandshakePacket packet)
    {
        session.Protocol = (NetworkProtocol)packet.NextState;
    }
}