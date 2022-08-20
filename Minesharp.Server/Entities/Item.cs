using Minesharp.Server.Entities.Metadata;
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
    
    public Item(World world) : base(world)
    {
        
    }

    public override void Tick()
    {
        
    }

    public override void Update()
    {
        
    }

    public override IEnumerable<GamePacket> GetSpawnPackets()
    {
        return new GamePacket[]
        {
            new SpawnEntityPacket(Id, UniqueId, 45, Position),
            new EntityMetadataPacket(Id, Metadata.GetEntries()),
            new EntityTeleportPacket(Id, Position),
            new EntityVelocityPacket(Id, Velocity)
        };
    }
}