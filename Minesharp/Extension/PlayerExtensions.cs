using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class PlayerExtensions
{
    public static void SendHealth(this Player player)
    {
        var health = (float)(player.Health / player.MaximumHealth * 20);
        player.SendPacket(new HealthPacket(health, player.Food, player.Saturation));
    }

    public static void SendPosition(this Player player)
    {
        player.SendPacket(new SyncPositionPacket(player.Position, player.Rotation));
    }

    public static void SendInventory(this Player player)
    {
        player.SendPacket(new UpdateInventoryContentPacket
        {
            Window = 0,
            State = 10,
            Items = player.Inventory.GetContent()
        });
    }

    public static void SendEntityAnimation(this Player player, Entity entity, Animation animation)
    {
        player.SendPacket(new EntityAnimationPacket
        {
            EntityId = entity.Id,
            Animation = animation
        });
    }

    public static void SendPlayerList(this Player player)
    {
        var server = player.Server;
        var players = server.GetPlayers();
        var infos = new List<PlayerInfo>();

        foreach (var value in players)
        {
            infos.Add(new PlayerInfo
            {
                Id = value.UniqueId,
                Ping = 5,
                Username = value.Username,
                GameMode = value.GameMode
            });
        }

        player.SendPacket(new PlayerListPacket
        {
            Action = PlayerListAction.AddPlayer,
            Players = infos
        });
    }
}