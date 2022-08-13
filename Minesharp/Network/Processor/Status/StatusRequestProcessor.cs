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
            Version = new StatusVersion
            {
                Name = Server.Version,
                Protocol = Server.Protocol
            },
            Players = new StatusPlayers
            {
                Max = server.MaxPlayers,
                Online = server.GetPlayers().Count()
            },
            Description = new StatusDescription
            {
                Text = server.Description
            }
        });
    }
}