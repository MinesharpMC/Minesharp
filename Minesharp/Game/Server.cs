using System.Collections.Concurrent;
using Minesharp.Chat.Component;
using Minesharp.Configuration;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Game;

public sealed class Server
{
    private readonly WorldManager worldManager;
    private readonly Scheduler scheduler;
    private readonly SessionManager sessionManager;
    private readonly ServerConfiguration configuration;

    public const string Version = "1.19";
    public const int Protocol = 759;

    private int lastEntityId;
    private byte tick;
    
    public byte Tps { get; private set; }
    public DateTime LastTpsUpdate { get; private set; }
    
    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;
    public byte ViewDistance => configuration.ViewDistance;

    public Scheduler Scheduler => scheduler;
    public WorldManager WorldManager => worldManager;
    
    public Server(ServerConfiguration configuration, Scheduler scheduler, WorldManager worldManager, SessionManager sessionManager)
    {
        this.configuration = configuration;
        this.scheduler = scheduler;
        this.worldManager = worldManager;
        this.sessionManager = sessionManager;
    }

    public int GetNextEntityId()
    {
        return ++lastEntityId;
    }

    public IEnumerable<Player> GetPlayers()
    {
        var output = new List<Player>();
        var worlds = worldManager.GetWorlds();
        foreach (var world in worlds)
        {
            output.AddRange(world.Players);
        }

        return output;
    }

    public void Tick()
    {
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
        
        scheduler.Tick();

        if (LastTpsUpdate.AddSeconds(1) <= DateTime.UtcNow)
        {
            Tps = tick;
            LastTpsUpdate = DateTime.UtcNow;
            tick = 0;
        }

        tick++;
    }
}