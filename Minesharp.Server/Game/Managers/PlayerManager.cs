using System.Collections.Concurrent;
using Minesharp.Server.Game.Entities;

namespace Minesharp.Server.Game.Managers;

public class PlayerManager
{
    private readonly ConcurrentDictionary<Guid, Player> players = new();
    private readonly ConcurrentDictionary<string, Player> playersByName = new();

    public void Add(Player player)
    {
        players[player.UniqueId] = player;
        playersByName[player.Username] = player;
    }

    public void Remove(Player player)
    {
        players.Remove(player.UniqueId, out _);
        playersByName.Remove(player.Username, out _);
    }

    public Player GetPlayer(Guid uniqueId)
    {
        return players.GetValueOrDefault(uniqueId);
    }

    public IEnumerable<Player> GetPlayers()
    {
        return players.Values;
    }
}