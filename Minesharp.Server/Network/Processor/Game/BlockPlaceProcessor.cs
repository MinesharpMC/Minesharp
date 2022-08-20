using Minesharp.Events.Block;
using Minesharp.Server.Extension;
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

        var item = session.Player.Inventory.ItemInHand;
        if (item == null)
        {
            return;
        }

        target.Type = item.Type;

        var e = player.Server.CallEvent(new BlockPlaceEvent(block, player));
        if (e.IsCancelled)
        {
            target.Type = Material.Air;
        }
        else
        {
            item.Amount--;

            if (item.Amount == 0)
            {
                player.Inventory.ItemInHand = null;
                world.Broadcast(new EquipmentPacket(player.Id, EquipmentSlot.MainHand, player.Inventory.ItemInHand));
            }

            player.SendInventorySlot(player.Inventory.HandSlot);
        }
        
        player.SendAckBlockChange(packet.Sequence);
    }
}