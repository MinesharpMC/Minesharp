using Minesharp.Server.Network.Packet.Status.Client;
using Minesharp.Server.Network.Packet.Status.Server;

namespace Minesharp.Server.Network.Processor.Status;

public class StatusRequestProcessor : PacketProcessor<StatusRequestPacket>
{
    private readonly GameServer server;

    public StatusRequestProcessor(GameServer server)
    {
        this.server = server;
    }

    protected override void Process(NetworkSession session, StatusRequestPacket packet)
    {
        session.SendPacket(new StatusResponsePacket
        {
            Version = new StatusVersion
            {
                Name = GameServer.Version,
                Protocol = GameServer.Protocol
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