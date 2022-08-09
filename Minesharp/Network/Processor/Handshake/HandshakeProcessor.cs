using Minesharp.Network.Packet.Client.Handshake;

namespace Minesharp.Network.Processor.Handshake;

public class HandshakeProcessor : PacketProcessor<HandshakePacket>
{
    protected override void Process(NetworkClient client, HandshakePacket packet)
    {
        client.Protocol = (NetworkProtocol)packet.NextState;
    }
}