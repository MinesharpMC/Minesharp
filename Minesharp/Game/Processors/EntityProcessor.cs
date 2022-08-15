using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Game.Processors;

public class EntityProcessor
{
    private readonly Player player;
    private readonly HashSet<Guid> knownEntities = new();

    public EntityProcessor(Player player)
    {
        this.player = player;
    }

    public void Tick()
    {
        var world = player.World;
        foreach (var chunkKey in player.KnownChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            var entities = chunk.GetEntities();
            foreach (var entity in entities)
            {
                if (entity == player || knownEntities.Contains(entity.UniqueId))
                {
                    continue;
                }

                if (entity.Type == EntityType.Player)
                {
                    player.SendPacket(new SpawnPlayerPacket
                    {
                        Id = entity.Id,
                        UniqueId = entity.UniqueId,
                        Position = entity.Position,
                        Rotation = entity.Rotation
                    });
                    
                    knownEntities.Add(entity.UniqueId);
                }
            }
        }

        foreach (var chunkKey in player.OutdatedChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            if (chunk is null)
            {
                Log.Error("Chunk is null can't despawn entities");
                continue;
            }
            
            var entities = chunk.GetEntities();
            var removedEntities = new List<int>();
            foreach (var entity in entities)
            {
                if (entity == player || !knownEntities.Contains(entity.UniqueId))
                {
                    continue;
                }
                
                removedEntities.Add(entity.Id);
            }
            
            player.SendRemoveEntities(removedEntities);
        }

        foreach (var entityId in knownEntities)
        {
            var entity = world.GetEntity(entityId);
            if (entity is null)
            {
                continue;
            }
            
            if (entity.Moved && entity.Rotated)
            {
                player.SendEntityMoveAndRotate(entity);
            }
            else if (entity.Moved)
            {
                player.SendEntityMove(entity);
            }
            else if (entity.Rotated)
            {
                player.SendEntityRotate(entity);
            }
        }
    }
}