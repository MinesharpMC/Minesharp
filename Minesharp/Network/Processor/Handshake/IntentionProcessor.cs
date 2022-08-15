using Minesharp.Chat.Component;
using Minesharp.Common.Enum;
using Minesharp.Game;
using Minesharp.Packet.Handshake.Client;
using Minesharp.Packet.Login.Server;

namespace Minesharp.Network.Processor.Handshake;

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

        if (packet.ProtocolVersion != Server.Protocol)
        {
            session.SendPacket(new DisconnectPacket
            {
                Reason = new TextComponent
                {
                    Text = packet.ProtocolVersion < Server.Protocol 
                        ? $"Outdated client! (I'm running on {Server.Version})"
                        : $"Outdated server! (I'm running on {Server.Version})"
                }
            });
            session.Disconnect();
        }
    }
}