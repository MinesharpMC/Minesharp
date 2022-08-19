using Minesharp.Common.Enum;
using Minesharp.Game.Broadcast;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class ChangeHeldItemProcessor : PacketProcessor<ChangeHeldItemPacket>
{
    protected override void Process(NetworkSession session, ChangeHeldItemPacket packet)
    {
        var player = session.Player;
        var inventory = player.Inventory;
        var slot = (short)(packet.Slot + 36);

        inventory.MainHandSlot = slot;

        player.World.Broadcast(new EquipmentPacket(player.Id, EquipmentSlot.MainHand, inventory.ItemInMainHand), new CanSeeEntityRule(player));
    }
}