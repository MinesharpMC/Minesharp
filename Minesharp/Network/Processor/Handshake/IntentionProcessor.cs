using Minesharp.Packet.Handshake.Client;

namespace Minesharp.Network.Processor.Handshake;

public class IntentionProcessor : PacketProcessor<IntentionPacket>
{
    protected override void Process(NetworkSession session, IntentionPacket packet)
    {
        session.Protocol = packet.RequestedProtocol;
    }
}