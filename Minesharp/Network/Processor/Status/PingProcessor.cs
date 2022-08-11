using Minesharp.Network.Packet.Client.Status;
using Minesharp.Network.Packet.Server.Status;

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