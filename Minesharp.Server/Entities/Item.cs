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
        var entities = GetNearbyEntities(1, 0.5, 1);
        foreach (var entity in entities)
        {
            if (entity is not Player player)
            {
                continue;
            }

            var remaining = player.Inventory.AddItem(ItemStack);
            
            World.Broadcast(new CollectItemPacket
            {
                CollectedId = Id,
                CollectorId = player.Id,
                Count = ItemStack.Amount
            });
            
            if (remaining == null)
            {
                World.RemoveEntity(this);
            }
            else
            {
                ItemStack = remaining;
            }
        }
    }

    public override IEnumerable<GamePacket> GetSpawnPackets()
    {
        return new GamePacket[]
        {
            new SpawnEntityPacket(Id, UniqueId, 44, Position),
            new EntityMetadataPacket(Id, Metadata.GetEntries()),
            // new EntityTeleportPacket(Id, Position),
            // new EntityVelocityPacket(Id, Velocity)
        };
    }
}