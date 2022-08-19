using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

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