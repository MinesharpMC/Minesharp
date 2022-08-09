using System.Text.Json;
using Minesharp.Network.Packet.Client.Status;
using Minesharp.Network.Packet.Server.Status;

namespace Minesharp.Network.Processor.Status;

public class RequestProcessor : PacketProcessor<RequestPacket>
{
    protected override void Process(NetworkClient client, RequestPacket packet)
    {
        client.SendPacket(new ResponsePacket
        {
            Json = JsonSerializer.Serialize(new
            {
                version = new
                {
                    name = "1.19",
                    protocol = 759
                },
                players = new
                {
                    max = 1000,
                    online = 0
                },
                description = new
                {
                    text = "Powered by Minesharp"
                }
            })
        });
    }
}