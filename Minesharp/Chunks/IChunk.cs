using Minesharp.Entities;
using Minesharp.Worlds;

namespace Minesharp.Chunks;

public interface IChunk
{
    public int X { get; }
    public int Z { get; }

    IWorld GetWorld();

    IEnumerable<IEntity> GetEntities();
    IEnumerable<IPlayer> GetPlayers();
}