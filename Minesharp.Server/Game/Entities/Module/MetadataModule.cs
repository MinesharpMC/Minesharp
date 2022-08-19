using Minesharp.Server.Game.Entities.Meta;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Game.Entities.Module;

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