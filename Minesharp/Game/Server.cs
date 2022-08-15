using Minesharp.Configuration;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Game.Managers;
using Minesharp.Game.Schedule;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Packet;

namespace Minesharp.Game;

public sealed class Server
{
    private readonly WorldManager worldManager;
    private readonly Scheduler scheduler;
    private readonly SessionManager sessionManager;
    private readonly PlayerManager playerManager;
    private readonly ServerConfiguration configuration;

    public const string Version = "1.19";
    public const int Protocol = 759;

    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;
    public byte ViewDistance => configuration.ViewDistance;

    private static int lastEntityId;

    public Server(ServerConfiguration configuration, SessionManager sessionManager)
    {
        this.configuration = configuration;
        this.sessionManager = sessionManager;
        this.worldManager = new WorldManager();
        this.scheduler = new Scheduler();
        this.playerManager = new PlayerManager();
    }

    public static int GetNextEntityId()
    {
        return ++lastEntityId;
    }

    public World CreateWorld(WorldCreator creator)
    {
        return worldManager.CreateWorld(creator);
    }
    
    public void Broadcast(IPacket packet)
    {
        var players = playerManager.GetPlayers();
        foreach (var player in players)
        {
            player.SendPacket(packet);
        }
    }
    
    public void Broadcast(IPacket packet, params IBroadcastRule[] rules)
    {
        var players = playerManager.GetPlayers();
        foreach (var player in players)
        {
            if (rules.Any(x => !x.IsAllowed(player)))
            {
                continue;
            }
            
            player.SendPacket(packet);
        }
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playerManager.GetPlayers();
    }

    public IEnumerable<World> GetWorlds()
    {
        return worldManager.GetWorlds();
    }

    public World GetDefaultWorld()
    {
        return worldManager.GetDefaultWorld();
    }

    public void AddPlayer(Player player)
    {
        playerManager.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        playerManager.Remove(player);
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
    }
}