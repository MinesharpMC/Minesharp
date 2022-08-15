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
    private readonly HashSet<int> knownEntities = new();

    public IReadOnlySet<int> KnownEntities => knownEntities;

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
                if (entity == player || knownEntities.Contains(entity.Id))
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
                    
                    knownEntities.Add(entity.Id);
                }
            }
        }

        var removedEntities = new List<int>();
        foreach (var chunkKey in player.OutdatedChunks)
        {
            var chunk = world.GetChunk(chunkKey);
            if (chunk is null)
            {
                Log.Error("Chunk is null can't despawn entities");
                continue;
            }
            
            var entities = chunk.GetEntities();
            foreach (var entity in entities)
            {
                if (knownEntities.Contains(entity.Id))
                {
                    removedEntities.Add(entity.Id);
                }
            }
        }

        foreach (var entityId in knownEntities)
        {
            var entity = world.GetEntity(entityId);
            if (entity is null)
            {
                removedEntities.Add(entityId);
                continue;
            }

            var delta = entity.Position.Delta(entity.LastPosition);
            var teleport = delta.X > short.MaxValue || delta.Y > short.MaxValue || delta.Z > short.MaxValue 
                           || delta.X < short.MinValue || delta.Y < short.MinValue || delta.Z < short.MinValue;

            switch (entity.Moved)
            {
                case true when teleport:
                    player.SendEntityTeleport(entity);
                    break;
                case true when entity.Rotated:
                    player.SendEntityMoveAndRotate(entity);
                    break;
                case true:
                    player.SendEntityMove(entity);
                    break;
                case false when entity.Rotated:
                    player.SendEntityRotate(entity);
                    break;
            }
        }

        if (removedEntities.Any())
        {
            player.SendRemoveEntities(removedEntities);
            foreach (var removedEntity in removedEntities)
            {
                knownEntities.Remove(removedEntity);
            }
        }
    }
}