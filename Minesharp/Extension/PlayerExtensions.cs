using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class PlayerExtensions
{
    public static void SendPosition(this Player player)
    {
        player.SendPacket(new SyncPositionPacket
        {
            Position = player.Position,
            Rotation = player.Rotation
        });
    }

    public static void SendEntityMoveAndRotate(this Player player, Entity entity)
    {
        player.SendPacket(new UpdateEntityPositionAndRotationPacket
        {
            EntityId = entity.Id,
            Delta = entity.Position.Delta(entity.LastPosition),
            Angle = entity.Rotation,
            IsGrounded = true
        });
    }

    public static void SendEntityMove(this Player player, Entity entity)
    {
        player.SendPacket(new UpdateEntityPositionPacket
        {
            EntityId = entity.Id,
            Delta = entity.Position.Delta(entity.LastPosition),
            IsGrounded = true
        });
    }

    public static void SendRemoveEntities(this Player player, IList<int> entities)
    {
        player.SendPacket(new RemoveEntitiesPacket
        {
            Entities = entities
        });
    }
    
    public static void SendEntityRotate(this Player player, Entity entity)
    {
        player.SendPacket(new UpdateEntityRotationPacket
        {
            EntityId = entity.Id,
            Angle = entity.Rotation,
            IsGrounded = true
        });
    }

    public static void SendPlayerList(this Player player)
    {
        var server = player.Server;
        var players = server.GetPlayers();
        var infos = new List<PlayerInfo>();

        foreach (var value in players)
        {
            infos.Add(new PlayerInfo
            {
                Id = value.UniqueId,
                Ping = 5,
                Username = value.Username,
                GameMode = value.GameMode
            });
        }
        
        player.SendPacket(new PlayerListPacket
        {
            Action = PlayerListAction.AddPlayer,
            Players = infos
        });
    }
}