using System.Collections.Concurrent;

namespace Minesharp.Game.Worlds;

public class WorldManager
{
    private readonly ConcurrentDictionary<string, World> worlds = new();
    
    public World CreateWorld(WorldCreator creator)
    {
        var world = GetWorld(creator.GetWorldName());
        if (world is not null)
        {
            return world;
        }

        worlds[creator.GetWorldName()] = world = new World(creator);
        return world;
    }

    public World GetWorld(string name)
    {
        return worlds.GetValueOrDefault(name);
    }

    public IEnumerable<World> GetWorlds()
    {
        return worlds.Values;
    }

    public World GetPrimaryWorld()
    {
        return worlds.Values.FirstOrDefault();
    }
}