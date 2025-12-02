using Minesharp.Entities;
using Minesharp.Events;
using Minesharp.Server.Blocks;
using Minesharp.Server.Blocks.Type;
using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Common.Configuration;
using Minesharp.Server.Entities;
using Minesharp.Server.Network;
using Minesharp.Server.Network.Packet;
using Minesharp.Server.Plugins;
using Minesharp.Server.Worlds;
using Minesharp.Worlds;

namespace Minesharp.Server;

public sealed class GameServer : IServer
{
    public const int Protocol = 759;
    public const int TickRate = 100;
    public const string Version = "1.19";

    private readonly WorldManager worldManager;
    private readonly SessionManager sessionManager;
    private readonly ServerConfiguration configuration;
    private readonly BlockRegistry blockRegistry;
    private readonly PluginManager pluginManager;

    private readonly long[] ticks = new long[TickRate];

    private int entityId;
    private long nextTick;
    private int tickCounter;

    public GameServer(ServerConfiguration configuration, SessionManager sessionManager, BlockRegistry blockRegistry)
    {
        this.configuration = configuration;
        this.sessionManager = sessionManager;
        this.worldManager = new WorldManager(this);
        this.blockRegistry = blockRegistry;
        this.pluginManager = new PluginManager(this);
    }

    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;
    public byte ViewDistance => configuration.ViewDistance;
    public IEnumerable<World> Worlds => worldManager.GetWorlds();
    public PluginManager PluginManager => pluginManager;
    public BlockRegistry BlockRegistry => blockRegistry;

    public int Tps { get; private set; }

    public IEnumerable<IWorld> GetWorlds()
    {
        return Worlds;
    }

    public int GetNextEntityId()
    {
        return ++entityId;
    }

    public World CreateWorld(WorldCreator creator)
    {
        return worldManager.CreateWorld(creator);
    }
    
    public BlockType GetBlockFrom(Material material)
    {
        return blockRegistry.GetBlockType(material);
    }

    public void Broadcast(IPacket packet)
    {
        var worlds = worldManager.GetWorlds();
        foreach (var world in worlds)
        {
            var players = world.GetEntities<Player>();
            foreach (var player in players)
            {
                player.SendPacket(packet);
            }
        }
    }

    public void Broadcast(IPacket packet, params IBroadcastRule[] rules)
    {
        var worlds = worldManager.GetWorlds();
        foreach (var world in worlds)
        {
            var players = world.GetEntities<Player>();
            foreach (var player in players)
            {
                if (!rules.All(x => x.IsAllowed(player)))
                {
                    continue;
                }
                
                player.SendPacket(packet);
            }
        }
    }

    public IEnumerable<Player> GetPlayers()
    {
        var players = worldManager
            .GetWorlds()
            .SelectMany(x => x.GetEntities<Player>())
            .ToList();
        
        return players;
    }

    public T SendEvent<T>(T e) where T : IEvent
    {
        PluginManager.CallEvent(e);
        return e;
    }

    public World GetDefaultWorld()
    {
        return worldManager.GetDefaultWorld();
    }

    public void Tick()
    {
        var current = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var elapsed = current - nextTick;

        if (elapsed < 1000 / TickRate)
        {
            return;
        }

        var sessions = sessionManager.GetSessions();
        foreach (var session in sessions)
        {
            session.Tick();
        }

        var worlds = worldManager.GetWorlds();
        foreach (var world in worlds)
        {
            world.Tick();
        }

        nextTick = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        ticks[tickCounter] = elapsed;
        tickCounter++;

        if (tickCounter >= ticks.Length)
        {
            Tps = (int)(1000 / ticks.Average());
            tickCounter = 0;
        }
    }
}