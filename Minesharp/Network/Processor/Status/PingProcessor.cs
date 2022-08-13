using Minesharp.Packet.Status.Client;
using Minesharp.Packet.Status.Server;

namespace Minesharp.Network.Processor.Status;

public class PingProcessor : PacketProcessor<PingPacket>
{
    protected override void Process(NetworkSession session, PingPacket packet)
    {
        session.SendPacket(new PongPacket
        {
            Payload = packet.Payload
        });
    }
}