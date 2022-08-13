using System.Collections.Concurrent;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;

namespace Minesharp.Game.Worlds;

public sealed class World : IEquatable<World>
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

        chunkFactory = new ChunkFactory(creator.ChunkGenerator, this);

        playersById = new ConcurrentDictionary<Guid, Player>();
        playersByName = new ConcurrentDictionary<string, Player>();
        chunks = new ConcurrentDictionary<ChunkKey, Chunk>();
    }

    public bool Equals(World other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is World other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(World left, World right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(World left, World right)
    {
        return !Equals(left, right);
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }
    
    public Chunk GetChunkAt(Position position)
    {
        return GetChunkAt(position.BlockX >> 4, position.BlockZ >> 4);
    }

    public Chunk GetChunkAt(int x, int z)
    {
        return GetChunk(ChunkKey.Of(x, z));
    }
    
    public Chunk GetChunk(ChunkKey key)
    {
        var chunk = chunks.GetValueOrDefault(key);
        if (chunk is null)
        {
            chunks[key] = chunk = chunkFactory.Create(key);
        }

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

        playersById[player.UniqueId] = player;
        playersByName[player.Username] = player;
    }

    public void Remove(Player player)
    {
        player.World = null;

        playersById.Remove(player.UniqueId, out _);
        playersByName.Remove(player.Username, out _);
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playersById.Values;
    }
}