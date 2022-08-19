using Minesharp.Chat.Component;
using Minesharp.Server.Game;
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

        if (packet.ProtocolVersion != GameServer.Protocol)
        {
            session.SendPacket(new DisconnectPacket
            {
                Reason = new TextComponent
                {
                    Text = packet.ProtocolVersion < GameServer.Protocol
                        ? $"Outdated client! (I'm running on {GameServer.Version})"
                        : $"Outdated server! (I'm running on {GameServer.Version})"
                }
            });
            session.Disconnect();
        }
    }
}