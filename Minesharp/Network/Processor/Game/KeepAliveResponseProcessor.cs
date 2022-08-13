using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class KeepAliveResponseProcessor : PacketProcessor<KeepAliveResponsePacket>
{
    protected override void Process(NetworkSession session, KeepAliveResponsePacket packet)
    {
        if (session.LastKeepAlive != packet.Timestamp)
        {
            return;
        }
        
        session.LastKeepAliveReceivedAt = DateTime.UtcNow;
    }
}