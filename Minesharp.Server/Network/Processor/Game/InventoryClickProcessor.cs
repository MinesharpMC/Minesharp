using System.Collections.Concurrent;
using Minesharp.Server.Entities;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Storages;
using Minesharp.Storages;
using Serilog;

namespace Minesharp.Server.Network.Processor.Game;

public class InventoryClickProcessor : PacketProcessor<InventoryClickPacket>
{
    private static readonly ConcurrentDictionary<Player, ItemStack> Cursors = new();
    
    protected override void Process(NetworkSession session, InventoryClickPacket packet)
    {
        var player = session.Player;
        var inventory = player.Inventory;
        
        Cursors[player] = packet.CarriedItem;
        
        foreach (var (slotIndex, stack) in packet.Items)
        {
            var slot = inventory.GetSlot(slotIndex);
            if (slot == null)
                continue;

            slot.Item = stack;
            
            session.SendPacket(new UpdateInventorySlotPacket
            {
                Window = packet.Window,
                Slot = slotIndex,
                State = 0,
                Item = stack
            });
        }
    }
}