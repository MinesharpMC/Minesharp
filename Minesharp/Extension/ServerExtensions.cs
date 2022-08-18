using Minesharp.Chat;
using Minesharp.Chat.Component;
using Minesharp.Game;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
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

    public static void BroadcastMessage(this Server server, string message, ChatColor color)
    {
        server.BroadcastMessage(new TextComponent
        {
            Text = message,
            Color = color
        });
    }

    public static void BroadcastPlayerListAdd(this Server server, Player player)
    {
        server.Broadcast(new PlayerListPacket
        {
            Action = PlayerListAction.AddPlayer,
            Players = new List<PlayerInfo>
            {
                new()
                {
                    Id = player.UniqueId,
                    Ping = 5,
                    Username = player.Username,
                    GameMode = player.GameMode
                }
            }
        }, new ExceptPlayerRule(player));
    }

    public static void BroadcastPlayerListRemove(this Server server, Player player)
    {
        server.Broadcast(new PlayerListPacket
        {
            Action = PlayerListAction.RemovePlayer,
            Players = new List<PlayerInfo>
            {
                new()
                {
                    Id = player.UniqueId
                }
            }
        }, new ExceptPlayerRule(player));
    }
}