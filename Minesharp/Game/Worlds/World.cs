using System.Collections.Concurrent;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Game.Managers;
using Minesharp.Packet;
using Serilog;

namespace Minesharp.Game.Worlds;

public sealed class World
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
        
        playerManager = new PlayerManager();
        entityManager = new EntityManager();
        chunkManager = new ChunkManager(new ChunkFactory(creator.ChunkGenerator, this));
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }
    public GameMode GameMode { get; }
    
    public Block GetBlockAt(int x, int y, int z)
    {
        return new Block
        {
            World = this,
            X = x,
            Y = y,
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
        var entities = entityManager.GetEntities();
        foreach (var entity in entities)
        {
            entity.Tick();
        }

        foreach (var entity in entities) // Update all entities last position (need to be done after all entities tick)
        {
            entity.LastPosition = entity.Position;
            entity.LastRotation = entity.Rotation;
        }
        
        var chunks = chunkManager.GetChunks();
        foreach (var chunk in chunks)
        {
            if (!chunk.IsLocked)
            {
                chunkManager.UnloadChunk(chunk);
                continue;
            }
            
            chunk.Tick();
        }
    }
}