using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;

namespace Minesharp.Game;

public class Server
{
    private readonly PlayerManager playerManager;
    private readonly WorldManager worldManager;

    public Server(PlayerManager playerManager, WorldManager worldManager)
    {
        this.playerManager = playerManager;
        this.worldManager = worldManager;
    }

    public World GetWorld(string name)
    {
        return worldManager.GetWorld(name);
    }

    public World CreateWorld(WorldCreator creator)
    {
        return worldManager.CreateWorld(creator);
    }

    public IEnumerable<World> GetWorlds()
    {
        return worldManager.GetWorlds();
    }

    public Player GetPlayer(Guid id)
    {
        return playerManager.Get(id);
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playerManager.GetPlayers();
    }

    public void RemovePlayer(Player player)
    {
        playerManager.Remove(player);
    }

    public void AddPlayer(Player player)
    {
        playerManager.Add(player);
    }

    public void BroadcastMessage(string message)
    {
        foreach (var player in playerManager.GetPlayers())
        {
            player.SendMessage(message);
        }
    }

    public void Tick()
    {
        var players = playerManager.GetPlayers();
        foreach (var player in players)
        {
            player.Tick();
        }
    }
}