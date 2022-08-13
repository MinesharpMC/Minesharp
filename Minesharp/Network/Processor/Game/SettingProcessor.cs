using Minesharp.Game;
using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class SettingProcessor : PacketProcessor<SettingPacket>
{
    protected override void Process(NetworkSession session, SettingPacket packet)
    {
        session.Player.Setting = new Setting
        {
            Locale = packet.Locale,
            ChatColor = packet.ChatColor,
            ChatMode = packet.ChatMode,
            ViewDistance = packet.ViewDistance,
            DisplayedSkinSections = packet.DisplayedSkinSections,
            MainHand = packet.MainHand
        };
    }
}