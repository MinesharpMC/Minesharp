using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class SettingProcessor : PacketProcessor<SettingPacket>
{
    protected override void Process(NetworkSession session, SettingPacket packet)
    {
        session.Player.Locale = packet.Locale;
        session.Player.MainHand = packet.MainHand;
        session.Player.ViewDistance = Math.Min(packet.ViewDistance, session.Player.Server.ViewDistance);
    }
}