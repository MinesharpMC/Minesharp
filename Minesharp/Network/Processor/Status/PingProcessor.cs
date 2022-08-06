using Minesharp.Network.Packet.Client.Status;
using Minesharp.Network.Packet.Server.Status;

namespace Minesharp.Network.Processor.Status;

public class PingProcessor : PacketProcessor<PingPacket>
{
    protected override void Process(NetworkClient client, PingPacket packet)
    {
        client.SendPacket(new PongPacket
        {
            Payload = packet.Payload
        });
    }
}