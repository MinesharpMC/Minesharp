
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Processors;

public class EntityProcessor
{
    private readonly Player player;
    private readonly HashSet<int> knownEntities = new();

    public EntityProcessor(Player player)
    {
        this.player = player;
    }

    public void Tick()
    {
        var world = player.World;

        var removedEntities = new HashSet<int>(knownEntities);

        var entities = world.GetEntities();
        foreach (var entity in entities)
        {
            var canSee = player.CanSee(entity);
            if (!canSee)
            {
                continue;
            }

            if (!knownEntities.Contains(entity.Id))
            {
                if (entity.Type == EntityType.Player)
                {
                    player.SendPacket(new SpawnPlayerPacket
                    {
                        Id = entity.Id,
                        UniqueId = entity.UniqueId,
                        Position = entity.Position,
                        Rotation = entity.Rotation
                    });
                }

                knownEntities.Add(entity.Id);
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

            removedEntities.Remove(entity.Id);
        }
        
        if (removedEntities.Any())
        {
            player.SendRemoveEntities(removedEntities.ToList());
        }
    }
}