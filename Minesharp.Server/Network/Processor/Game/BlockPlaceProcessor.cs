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
            Log.Warning("Player {Name} tried to place block on non-air block", player.Username);
            return;
        }

        var equipmentSlot = packet.Hand == Hand.MainHand 
            ? EquipmentSlot.MainHand 
            : EquipmentSlot.OffHand;
        
        var slot = player.Inventory.GetEquipmentSlot(equipmentSlot);
        if (slot.Item == null)
        {
            Log.Warning("Player {Name} tried to place block without item in hand ({Hand})", player.Username, packet.Hand);
            return;
        }

        target.Type = slot.Item.Type;

        var e = player.Server.Publish(new BlockPlaceEvent(block, player));
        if (e.IsCancelled)
        {
            target.Type = Material.Air;
        }
        else
        {
            slot.Item.Amount--;

            if (slot.Item.Amount == 0)
            {
                slot.Item = null;
                world.Broadcast(new EquipmentPacket(player.Id, equipmentSlot, slot.Item));
            }

            player.UpdateInventorySlot(slot.Index);
        }
        
        player.SendAckBlockChange(packet.Sequence);
    }
}