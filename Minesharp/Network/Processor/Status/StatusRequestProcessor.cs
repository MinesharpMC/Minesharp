using Minesharp.Game;
using Minesharp.Packet.Status.Client;
using Minesharp.Packet.Status.Server;
using Newtonsoft.Json;

namespace Minesharp.Network.Processor.Status;

public class StatusRequestProcessor : PacketProcessor<StatusRequestPacket>
{
    private readonly Server server;

    public StatusRequestProcessor(Server server)
    {
        this.server = server;
    }

    protected override void Process(NetworkSession session, StatusRequestPacket packet)
    {
        session.SendPacket(new StatusResponsePacket
        {
            Json = JsonConvert.SerializeObject(new
            {
                version = new
                {
                    name = Server.Version,
                    protocol = Server.Protocol
                },
                players = new
                {
                    max = server.MaxPlayers,
                    online = server.GetPlayers().Count()
                },
                description = new
                {
                    text = server.Description
                }
            })
        });
    }
}