using Minesharp.Game;
using Minesharp.Network.Common;
using Minesharp.Network.Packet.Client.Handshake;

namespace Minesharp.Network.Processor.Handshake;

public class HandshakeProcessor : PacketProcessor<HandshakePacket>
{
    private readonly Server server;

    public HandshakeProcessor(Server server)
    {
        this.server = server;
    }

    protected override void Process(NetworkClient client, HandshakePacket packet)
    {
        if (server.Protocol != packet.Protocol)
        {
            return;
        }
        
        client.Protocol = (NetworkProtocol)packet.NextState;
    }
}