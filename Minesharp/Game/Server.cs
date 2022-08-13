using System.Collections.Concurrent;
using Minesharp.Configuration;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game;

public sealed class Server
{
    private readonly ServerConfiguration configuration;
    private readonly ConcurrentDictionary<string, World> worlds = new();

    public const string Version = "1.19";
    public const int Protocol = 759;

    private int lastEntityId;
    
    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;
    public byte ViewDistance => configuration.ViewDistance;
    
    public Server(ServerConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<Player> GetPlayers()
    {
        return GetWorlds().SelectMany(x => x.GetPlayers());
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

        return worlds[creator.Name] = new World(creator);
    }

    public World CreateWorld(string name)
    {
        return CreateWorld(new WorldCreator
        {
            Name = name
        });
    }

    public World GetWorld(string name)
    {
        return worlds.GetValueOrDefault(name);
    }

    public World GetWorld(Guid worldId)
    {
        return worlds.Values.FirstOrDefault(x => x.Id == worldId);
    }

    public int GetNextEntityId()
    {
        return ++lastEntityId;
    }

    public void Tick()
    {
        foreach (var world in GetWorlds())
        {
            var players = world.GetPlayers();
            foreach (var player in players)
            {
                player.Tick();
            }

            var chunks = world.GetChunks();
            foreach (var chunk in chunks)
            {
                while (chunk.ModifiedBlocks.TryDequeue(out var modifiedBlock))
                {
                    world.Broadcast(new BlockChangePacket
                    {
                        Position = modifiedBlock.Position,
                        Type = modifiedBlock.Type
                    });
                }
            }
        }
    }
}