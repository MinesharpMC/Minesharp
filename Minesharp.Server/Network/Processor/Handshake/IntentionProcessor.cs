using Minesharp.Chat.Component;
using Minesharp.Server.Network.Packet.Handshake.Client;
using Minesharp.Server.Network.Packet.Login.Server;

namespace Minesharp.Server.Network.Processor.Handshake;

public class IntentionProcessor : PacketProcessor<IntentionPacket>
{
    protected override void Process(NetworkSession session, IntentionPacket packet)
    {
        if (packet.RequestedProtocol != Protocol.Status && packet.RequestedProtocol != Protocol.Login)
        {
            session.Disconnect();
            return;
        }

        session.Protocol = packet.RequestedProtocol;

        if (packet.ProtocolVersion != Server.Game.GameServer.Protocol)
        {
            session.SendPacket(new DisconnectPacket
            {
                Reason = new TextComponent
                {
                    Text = packet.ProtocolVersion < Server.Game.GameServer.Protocol
                        ? $"Outdated client! (I'm running on {Server.Game.GameServer.Version})"
                        : $"Outdated server! (I'm running on {Server.Game.GameServer.Version})"
                }
            });
            session.Disconnect();
        }
    }
}