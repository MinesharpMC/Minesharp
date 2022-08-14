using System.Collections.Concurrent;

namespace Minesharp.Game.Worlds;

public class WorldManager
{
    private readonly ConcurrentDictionary<string, World> worlds = new();
    
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

        return worlds[creator.Name] = new World(creator);
    }

    public World GetWorld(string name)
    {
        return worlds.GetValueOrDefault(name);
    }
}