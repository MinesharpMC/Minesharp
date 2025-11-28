using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;
using Serilog;

namespace Minesharp.Server.Network.Processor.Game;

public class ChangeHeldItemProcessor : PacketProcessor<ChangeHeldItemPacket>
{
    protected override void Process(NetworkSession session, ChangeHeldItemPacket packet)
    {
        var player = session.Player;
        var inventory = player.Inventory;
        var slot = (short)(packet.Slot + 36);
        
        inventory.HandSlot = slot;

        player.World.Broadcast(new EquipmentPacket(player.Id, EquipmentSlot.MainHand, inventory.ItemInHand), new CanSeeEntityRule(player), new ExceptPlayerRule(player));
    }
}