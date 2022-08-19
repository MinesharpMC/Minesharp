using Minesharp.Server.Extension;
using Minesharp.Server.Game.Storages;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;
using Serilog;

namespace Minesharp.Server.Network.Processor.Game;

public class BlockPlaceProcessor : PacketProcessor<BlockPlacePacket>
{
    protected override void Process(NetworkSession session, BlockPlacePacket packet)
    {
        var player = session.Player;
        var world = player.World;
        var block = world.GetBlockAt(packet.Position);
        var target = block.GetRelative(packet.Face);

        if (target.Type != Material.Air)
        {
            return;
        }

        if (world.HasEntityAt(target.Position))
        {
            Log.Warning("Can't place block at {position} because and entity is here", target.Position);
            return;
        }

        var item = session.Player.Inventory.ItemInMainHand;
        if (item == null)
        {
            return;
        }

        item.Amount--;

        if (item.Amount == 0)
        {
            player.Inventory.ItemInMainHand = null;
            world.Broadcast(new EquipmentPacket(player.Id, EquipmentSlot.MainHand, player.Inventory.ItemInMainHand));
        }

        target.Type = item.Type;

        player.SendPacket(new AckBlockChangePacket(packet.Sequence));
        player.SendInventorySlot(player.Inventory.MainHandSlot);
    }
}