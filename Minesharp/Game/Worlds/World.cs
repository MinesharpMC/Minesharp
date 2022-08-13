using System.Collections.Concurrent;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Packet;

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

        chunkFactory = new ChunkFactory(creator.ChunkGenerator, this);

        playersById = new ConcurrentDictionary<Guid, Player>();
        playersByName = new ConcurrentDictionary<string, Player>();
        chunks = new ConcurrentDictionary<ChunkKey, Chunk>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }

    public Block GetBlockAt(int x, int y, int z)
    {
        return new Block
        {
            World = this,
            X = x,
            Y = Math.Min(256, Math.Max(y, 0)),
            Z = z
        };
    }

    public Material GetBlockTypeAt(Position position)
    {
        return GetBlockTypeAt(position.BlockX, position.BlockY, position.BlockZ);
    }

    public void SetBlockTypeAt(Position position, Material material)
    {
        SetBlockTypeAt(position.BlockX, position.BlockY, position.BlockZ, material);
    }
    
    public void SetBlockTypeAt(int x, int y, int z, Material material)
    {
        var chunk = GetChunkAt(x >> 4, z >> 4);
        chunk.SetTypeAt(x & 0xf, y, z & 0xf, material);
    }

    public Material GetBlockTypeAt(int x, int y, int z)
    {
        var chunk = GetChunkAt(x >> 4, z >> 4);
        return chunk.GetTypeAt(x, y, z);
    }

    public Block GetBlockAt(Position position)
    {
        return GetBlockAt(position.BlockX, position.BlockY, position.BlockZ);
    }
    
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

    public void Broadcast(IPacket packet)
    {
        foreach (var player in GetPlayers())
        {
            player.SendPacket(packet);
        }
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playersById.Values;
    }

    public IEnumerable<Chunk> GetChunks()
    {
        return chunks.Values;
    }
}