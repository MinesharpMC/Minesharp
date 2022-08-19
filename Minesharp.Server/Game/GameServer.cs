using Minesharp.Server.Configuration;
using Minesharp.Server.Game.Blocks;
using Minesharp.Server.Game.Broadcast;
using Minesharp.Server.Game.Entities;
using Minesharp.Server.Game.Managers;
using Minesharp.Server.Game.Schedule;
using Minesharp.Server.Game.Worlds;
using Minesharp.Server.Network.Packet;

namespace Minesharp.Server.Game;

public sealed class GameServer
{
    public const string Version = "1.19";
    public const int Protocol = 759;
    public const int TickRate = 125;
    private readonly WorldManager worldManager;
    private readonly SessionManager sessionManager;
    private readonly PlayerManager playerManager;
    private readonly ServerConfiguration configuration;
    private readonly BlockRegistry blockRegistry;

    private readonly long[] ticks = new long[TickRate];

    private int entityId;
    private long nextTick;
    private int tickCounter;

    public GameServer(ServerConfiguration configuration, SessionManager sessionManager, BlockRegistry blockRegistry)
    {
        this.configuration = configuration;
        this.sessionManager = sessionManager;
        worldManager = new WorldManager(this);
        Scheduler = new Scheduler();
        playerManager = new PlayerManager();
        this.blockRegistry = blockRegistry;
    }

    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;
    public byte ViewDistance => configuration.ViewDistance;

    public Scheduler Scheduler { get; }

    public BlockRegistry BlockRegistry => blockRegistry;

    public int Tps { get; private set; }

    public int GetNextEntityId()
    {
        return ++entityId;
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
            if (!rules.All(x => x.IsAllowed(player)))
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

        Scheduler.Tick();

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