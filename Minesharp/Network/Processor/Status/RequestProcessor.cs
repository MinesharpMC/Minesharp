using System.Text.Json;
using Minesharp.Game;
using Minesharp.Network.Packet.Client.Status;
using Minesharp.Network.Packet.Server.Status;

namespace Minesharp.Network.Processor.Status;

public class RequestProcessor : PacketProcessor<RequestPacket>
{
    private readonly Server server;

    public RequestProcessor(Server server)
    {
        this.server = server;
    }

    protected override void Process(NetworkClient client, RequestPacket packet)
    {
        client.SendPacket(new ResponsePacket
        {
            Json = JsonSerializer.Serialize(new
            {
                version = new
                {
                    name = server.Version,
                    protocol = server.Protocol
                },
                players = new
                {
                    max = server.MaxPlayers,
                    online = 0
                },
                description = new
                {
                    text = server.Description
                }
            })
        });
    }
}