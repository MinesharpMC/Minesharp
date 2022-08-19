using Minesharp.Blocks;
using Minesharp.Entities;

namespace Minesharp.Worlds;

public interface IWorld
{
    IEnumerable<IEntity> GetEntities();
    IEnumerable<IPlayer> GetPlayers();

    IBlock GetBlock(Position position);
    IEntity GetEntity(Guid uniqueId);
    IPlayer GetPlayer(Guid uniqueId);
}