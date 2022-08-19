using Minesharp.Server.Game.Entities;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Extension;

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
            State = 0,
            Items = player.Inventory.Content,
            HeldItem = player.Inventory.ItemInMainHand
        });
    }

    public static void SendAckBlockChange(this Player player, int sequence)
    {
        player.SendPacket(new AckBlockChangePacket
        {
            Sequence = sequence
        });
    }

    public static void SendInventorySlot(this Player player, short slot)
    {
        var item = player.Inventory[slot];
        player.SendPacket(new UpdateInventorySlotPacket
        {
            Window = 0,
            State = 0,
            Slot = slot,
            Item = item
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