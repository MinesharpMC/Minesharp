using System.Collections.Concurrent;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Game.Managers;
using Minesharp.Packet;
using Minesharp.Packet.Game;
using Serilog;

namespace Minesharp.Game.Worlds;

public sealed class World : IEquatable<World>
{
    private readonly PlayerManager playerManager;
    private readonly EntityManager entityManager;
    private readonly ChunkManager chunkManager;

    public World(WorldCreator creator)
    {
        Id = Guid.NewGuid();
        Name = creator.Name;
        Border = creator.Border;
        Difficulty = creator.Difficulty;
        GameMode = creator.GameMode;
        Random = new Random(Guid.NewGuid().GetHashCode());
        
        playerManager = new PlayerManager();
        entityManager = new EntityManager();
        chunkManager = new ChunkManager(new ChunkFactory(creator.ChunkGenerator, this));
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }
    public GameMode GameMode { get; }
    public Random Random { get; }
    
    public Block GetBlockAt(int x, int y, int z)
    {
        return new Block
        {
            World = this,
            Position = new Position(x, y, z)
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
        chunk.SetTypeAt(x, y, z, material);
    }

    public Material GetBlockTypeAt(int x, int y, int z)
    {
        var chunk = GetChunkAt(x >> 4, z >> 4);
        return chunk.GetTypeAt(x & 0xf, y, z & 0xf);
    }

    public Block GetBlockAt(Position position)
    {
        return GetBlockAt(position.BlockX, position.BlockY, position.BlockZ);
    }
    
    public Chunk GetChunkAt(Position position)
    {
        return GetChunkAt(position.BlockX, position.BlockZ);
    }

    public Chunk GetChunkAt(int x, int z)
    {
        return GetChunk(ChunkKey.Of(x >> 4, z >> 4));
    }

    public Chunk LoadChunk(ChunkKey key)
    {
        return chunkManager.LoadChunk(key);
    }

    public Chunk GetChunk(ChunkKey key)
    {
        return chunkManager.GetChunk(key);
    }

    public IEnumerable<Chunk> GetChunks()
    {
        return chunkManager.GetChunks();
    }

    public IEnumerable<Player> GetPlayers()
    {
        return playerManager.GetPlayers();
    }

    public IEnumerable<Entity> GetEntities()
    {
        return entityManager.GetEntities();
    }

    public void AddPlayer(Player player)
    {
        playerManager.Add(player);
        entityManager.Add(player);
    }
    
    public void Broadcast(GamePacket packet, params IBroadcastRule[] rules)
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

    public void RemovePlayer(Player player)
    {
        playerManager.Remove(player);
        entityManager.Remove(player);
    }

    public Entity GetEntity(Guid id)
    {
        return entityManager.GetEntity(id);
    }

    public Entity GetEntity(int id)
    {
        return entityManager.GetEntity(id);
    }

    public void Tick()
    {
        var chunks = chunkManager.GetChunks();
        var entities = entityManager.GetEntities();

        foreach (var entity in entities)
        {
            entity.Tick();
        }

        foreach (var entity in entities)
        {
            entity.Update();
        }
        
        foreach (var chunk in chunks)
        {
            chunk.Tick();
        }
    }

    public bool Equals(World other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
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
}