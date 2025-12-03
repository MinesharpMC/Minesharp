using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Storages;
using Serilog;

namespace Minesharp.Server.Network.Processor.Game;

public class InventoryClickProcessor : PacketProcessor<InventoryClickPacket>
{
    protected override void Process(NetworkSession session, InventoryClickPacket packet)
    {
        var player = session.Player;
        
        var inventory = player.OpenedInventory;
        if (inventory == null && packet.Window == 0)
        {
            inventory = player.OpenedInventory = new StorageView
            {
                Id = packet.Window,
                Inventory = player.Inventory,
                Cursor = packet.CarriedItem
            };
            
            Log.Information("Player {Name} opened inventory", player.Username);
        }

        if (inventory == null)
            return;

        if (packet.Slot == -999)
        {
            const double randomSpread = 0.02;
            const double forwardForce = 0.3;
            const double upwardForce = 0.15;

            // --- 1. Direction du joueur ---
            Vector lookDir = player.Rotation.ToDirection();

            // --- 2. Random à la vanilla ---
            double randX = (Random.Shared.NextDouble() - 0.5) * randomSpread;
            double randY = (Random.Shared.NextDouble() - 0.5) * randomSpread;
            double randZ = (Random.Shared.NextDouble() - 0.5) * randomSpread;

            // --- 3. Impulsion finale ---
            var velocity = new Vector(
                lookDir.X * forwardForce + randX,
                upwardForce + randY,
                lookDir.Z * forwardForce + randZ
            );
            
            player.World.DropItem(player.Position, inventory.Cursor, velocity);
        }
        
        inventory.Cursor = packet.CarriedItem;
        
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