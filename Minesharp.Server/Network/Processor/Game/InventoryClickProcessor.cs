using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class InventoryClickProcessor : PacketProcessor<InventoryClickPacket>
{
    protected override void Process(NetworkSession session, InventoryClickPacket packet)
    {
        var player = session.Player;
        var inventory = player.Inventory;

        var slot = inventory.GetSlot(packet.Slot);
        if (packet.Mode == 1)
        {
            if (packet.Button is 0 or 1)
            {
                var slots = inventory.GetSlots(slot.Type == SlotType.Container ? SlotType.QuickBar : SlotType.Container);
                var targetSlot = slots.FirstOrDefault(x => x.IsEmpty);
                if (targetSlot is null)
                {
                    return;
                }

                targetSlot.Item = slot.Item;
                slot.Item = null;
            }
        }
    }
}