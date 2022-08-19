using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Network.Processor.Game;

public class BlockPlaceProcessor : PacketProcessor<BlockPlacePacket>
{
    protected override void Process(NetworkSession session, BlockPlacePacket packet)
    {
        var player = session.Player;
        var world = player.World;
        var block = world.GetBlockAt(packet.Position);
        var target = block.GetRelative(packet.Face);

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

        if (target.Type == Material.Air)
        {
            target.Type = item.Type;
        }
        
        player.SendPacket(new AckBlockChangePacket(packet.Sequence));
        player.SendInventorySlot(player.Inventory.MainHandSlot);
    }
}