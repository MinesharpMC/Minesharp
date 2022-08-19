using System.Collections.Concurrent;
using Minesharp.Server.Game.Worlds;

namespace Minesharp.Server.Game.Managers;

public class WorldManager
{
    private readonly GameServer server;
    private readonly ConcurrentDictionary<string, World> worlds = new();

    public WorldManager(GameServer server)
    {
        this.server = server;
    }

    public World GetDefaultWorld()
    {
        return worlds.Values.First();
    }

    public IEnumerable<World> GetWorlds()
    {
        return worlds.Values;
    }

    public World CreateWorld(WorldCreator creator)
    {
        var world = GetWorld(creator.Name);
        if (world is not null)
        {
            return world;
        }

        return worlds[creator.Name] = new World(creator, server);
    }

    public World GetWorld(string name)
    {
        return worlds.GetValueOrDefault(name);
    }
}