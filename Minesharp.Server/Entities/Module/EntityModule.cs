using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Entities.Module;

public class EntityModule
{
    private readonly Player player;
    private readonly ChunkModule chunkModule;
    private readonly HashSet<int> entities = new();

    public EntityModule(Player player, ChunkModule chunkModule)
    {
        this.player = player;
        this.chunkModule = chunkModule;
    }

    public void Tick()
    {
        var world = player.World;

        var removedEntities = new HashSet<int>(entities);
        foreach (var entity in world.Entities)
        {
            if (entity == player)
            {
                continue;
            }

            var chunk = world.GetChunkAt(entity.Position);
            var chunkLoaded = chunkModule.HasLoaded(chunk);

            if (chunkLoaded)
            {
                if (!entities.Contains(entity.Id))
                {
                    var packets = entity.GetSpawnPackets();
                    foreach (var packet in packets)
                    {
                        player.SendPacket(packet);
                    }

                    entities.Add(entity.Id);
                }
                else
                {
                    var packets = entity.GetUpdatePackets();
                    foreach (var packet in packets)
                    {
                        player.SendPacket(packet);
                    }

                    removedEntities.Remove(entity.Id);
                }
            }
        }

        if (removedEntities.Count != 0)
        {
            player.SendPacket(new RemoveEntitiesPacket(removedEntities.ToList()));
            foreach (var removedEntity in removedEntities)
            {
                entities.Remove(removedEntity);
            }
        }
    }

    public bool HasLoaded(Entity entity)
    {
        return entities.Contains(entity.Id);
    }
}