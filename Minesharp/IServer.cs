using Minesharp.Entities;
using Minesharp.Events;
using Minesharp.Worlds;

namespace Minesharp;

public interface IServer
{
    int Tps { get; }
    
    IEnumerable<IWorld> GetWorlds();
    IEnumerable<IPlayer> GetPlayers();

    T CallEvent<T>(T e) where T : IEvent;
}