using Minesharp.Chat;
using Minesharp.Chat.Component;
using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Entities;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Extension;

public static class ServerExtensions
{
    public static void BroadcastMessage(this GameServer server, TextComponent message)
    {
        server.Broadcast(new SystemMessagePacket
        {
            Chat = message,
            IsOverlay = false
        });
    }

    public static void BroadcastMessage(this GameServer server, string message, ChatColor color)
    {
        server.BroadcastMessage(new TextComponent
        {
            Text = message,
            Color = color
        });
    }

    public static void BroadcastPlayerListAdd(this GameServer server, Player player)
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

    public static void BroadcastPlayerListRemove(this GameServer server, Player player)
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