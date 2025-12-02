using Minesharp.Entities;
using Minesharp.Worlds;

namespace Minesharp;

public interface IServer
{
    int Tps { get; }

    IEnumerable<IWorld> GetWorlds();
}