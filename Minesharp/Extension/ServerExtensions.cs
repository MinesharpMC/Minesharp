using Minesharp.Chat.Component;
using Minesharp.Game;
using Minesharp.Packet;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class ServerExtensions
{
    public static void BroadcastMessage(this Server server, TextComponent message)
    {
        server.Broadcast(new SystemMessagePacket
        {
            Chat = message,
            IsOverlay = false
        });
    }

    public static void Broadcast(this Server server, IPacket packet)
    {
        var players = server.GetPlayers();
        foreach (var player in players)
        {
            player.SendPacket(packet);
        }
    }
}