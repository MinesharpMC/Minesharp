using Minesharp.Server.Entities.Metadata;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Worlds;
using Minesharp.Storages;

namespace Minesharp.Server.Entities;

public class Item : Entity
{
    public ItemStack ItemStack
    {
        get => Metadata.Get<ItemStack>(MetadataIndex.Item);
        set => Metadata.Set(MetadataIndex.Item, value);
    }

    public Item(World world, Position position) : base(world, position)
    {
        Height = 0.25;
        Width = 0.25;
        Gravity = new Vector(0, -0.2, 0);
    }

    public override void Tick()
    {
        if (ItemStack.Amount <= 0)
            return;

        if (TicksLived < 100)
            return;
        
        var items = GetNearbyEntities<Item>(1, 1, 1);
        foreach (var other in items)
        {
            if (!ItemStack.CanStackWith(other.ItemStack))
                continue;
            
            var space = 64 - ItemStack.Amount;
            if (space <= 0)
                break;
            
            var amount = Math.Min(space, other.ItemStack.Amount);
            
            ItemStack.Amount += amount;
            other.ItemStack.Amount -= amount; 

            if (other.ItemStack.Amount <= 0)
            {
                World.RemoveEntity(other);
            }
            
            World.Broadcast(new EntityMetadataPacket(Id, Metadata.GetEntries()));
        }
        
        var players = GetNearbyEntities<Player>(1, 1, 1);
        foreach (var player in players)
        {
            var before = ItemStack.Amount;
            var remaining = player.Inventory.AddItem(ItemStack);
            var pickedUp = before - (remaining?.Amount ?? 0);

            if (pickedUp <= 0)
                continue;
            
            World.Broadcast(new CollectItemPacket
            {
                CollectedId = Id,
                CollectorId = player.Id,
                Count = pickedUp
            });
            
            if (remaining == null || remaining.Amount <= 0)
            {
                World.RemoveEntity(this);
                break;
            }

            ItemStack = remaining;
        }
    }

    public override IEnumerable<GamePacket> GetSpawnPackets()
    {
        return new GamePacket[]
        {
            new SpawnEntityPacket(Id, UniqueId, 44, Position),
            new EntityMetadataPacket(Id, Metadata.GetEntries()),
            // new EntityTeleportPacket(Id, Position),
            new EntityVelocityPacket(Id, Velocity)
        };
    }
}