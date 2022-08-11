using System.Collections.Concurrent;

namespace Minesharp.Game.Entities;

public class PlayerManager
{
    private readonly ConcurrentDictionary<Guid, Player> players = new();

    public IEnumerable<Player> GetPlayers()
    {
        return players.Values;
    }

    public Player Get(Guid id)
    {
        return players.GetValueOrDefault(id);
    }
    
    public void Add(Player player)
    {
        players[player.Id] = player;
    }

    public void Remove(Player player)
    {
        players.Remove(player.Id, out _);
    }
}