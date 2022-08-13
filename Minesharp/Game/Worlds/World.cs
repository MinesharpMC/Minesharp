using System.Collections.Concurrent;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;

namespace Minesharp.Game.Worlds;

public sealed class World
{
    private readonly ChunkFactory chunkFactory;
    private readonly ConcurrentDictionary<ChunkKey, Chunk> chunks;
    private readonly ConcurrentDictionary<Guid, Player> playersById;
    private readonly ConcurrentDictionary<string, Player> playersByName;

    public World(WorldCreator creator)
    {
        Id = Guid.NewGuid();
        Name = creator.Name;
        Border = creator.Border;
        Difficulty = creator.Difficulty;

        chunkFactory = new ChunkFactory(creator.ChunkGenerator);

        playersById = new ConcurrentDictionary<Guid, Player>();
        playersByName = new ConcurrentDictionary<string, Player>();
        chunks = new ConcurrentDictionary<ChunkKey, Chunk>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }

    public Chunk GetChunk(ChunkKey key)
    {
        var chunk = chunks.GetValueOrDefault(key);
        if (chunk is null) chunks[key] = chunk = chunkFactory.Create(key);

        return chunk;
    }

    public Player GetPlayer(string name)
    {
        return playersByName.GetValueOrDefault(name);
    }

    public Player GetPlayer(Guid playerId)
    {
        return playersById.GetValueOrDefault(playerId);
    }

    public void Add(Player player)
    {
        player.World = this;

        playersById[player.Id] = player;
        playersByName[player.Username] = player;
    }

    public void Remove(Player player)
    {
        player.World = null;

        playersById.Remove(player.Id, out _);
        playersByName.Remove(player.Username, out _);
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playersById.Values;
    }
}