using Minesharp.Game.Entities;
using Minesharp.Game.Entities.Meta;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Modules;

public class MetadataModule
{
    private readonly Player player;
    private readonly MetadataRegistry metadata;

    public MetadataModule(Player player)
    {
        this.player = player;
        metadata = player.Metadata;
    }

    public void Tick()
    {
        var changes = metadata.GetChanges();
        if (changes.Any())
        {
            player.SendPacket(new EntityMetadataPacket(player.Id, changes));
        }
    }

    public void Update()
    {
        // metadata.ClearChanges();
    }
}