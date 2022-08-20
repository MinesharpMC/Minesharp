using Minesharp.Blocks;
using Minesharp.Chunks;
using Minesharp.Entities;
using Minesharp.Server.Blocks;
using Minesharp.Server.Blocks.Type;
using Minesharp.Server.Chunks;
using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Entities;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game;
using Minesharp.Storages;
using Minesharp.Worlds;

namespace Minesharp.Server.Worlds;

public sealed class World : IEquatable<World>, IWorld
{
    private readonly PlayerManager playerManager;
    private readonly EntityManager entityManager;
    private readonly ChunkManager chunkManager;

    public World(WorldCreator creator, GameServer server)
    {
        Id = Guid.NewGuid();
        Name = creator.Name;
        Difficulty = creator.Difficulty;
        GameMode = creator.GameMode;
        SpawnPosition = creator.SpawnPosition;
        SpawnRotation = creator.SpawnRotation;
        Random = new Random(Guid.NewGuid().GetHashCode());
        Server = server;

        playerManager = new PlayerManager();
        entityManager = new EntityManager();
        chunkManager = new ChunkManager(new ChunkFactory(creator.ChunkGenerator, this));
    }

    public IEnumerable<Entity> Entities => entityManager.GetEntities();
    public IEnumerable<Player> Players => playerManager.GetPlayers();
    public Random Random { get; }
    public GameServer Server { get; }
    public Position SpawnPosition { get; set; }
    public Rotation SpawnRotation { get; set; }

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

    public Guid Id { get; }
    public string Name { get; }
    public Difficulty Difficulty { get; }
    public GameMode GameMode { get; }

    public IEnumerable<IEntity> GetEntities()
    {
        return Entities;
    }

    public IEnumerable<IPlayer> GetPlayers()
    {
        return Players;
    }

    public IBlock GetBlock(Position position)
    {
        return GetBlockAt(position);
    }

    public IEntity GetEntity(Guid uniqueId)
    {
        return entityManager.GetEntity(uniqueId);
    }

    public IPlayer GetPlayer(Guid uniqueId)
    {
        return playerManager.GetPlayer(uniqueId);
    }

    public IChunk GetChunk(Position position)
    {
        return GetChunkAt(position);
    }

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

    public void DropItem(Position position, ItemStack item)
    {
        var radius = 0.1;
        var offsetX = Random.NextDouble(radius * 2) - radius;
        var offsetY = 0.15;
        var offsetZ = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(offsetX, 2));
        
        if (Random.NextBoolean()) 
        {
            offsetZ *= -1;
        }

        var drop = new Item(this)
        {
            ItemStack = item,
            Position = new Position(position.X + 0.5, position.Y + 0.5, position.Z + 0.5),
        };
        
        entityManager.Add(drop);
    }

    public void SetBlockTypeAt(Position position, Material material)
    {
        SetBlockTypeAt(position.BlockX, position.BlockY, position.BlockZ, material);
    }

    public void SetBlockTypeAt(int x, int y, int z, Material material)
    {
        var chunk = GetChunkAt(x, z);
        var blockType = Server.BlockRegistry.GetBlockId(material);

        if (chunk is null)
        {
            return;
        }

        chunk.SetBlockType(x, y, z, blockType);
    }

    public bool HasEntityAt(Position position)
    {
        var entities = entityManager.GetEntities();
        foreach (var entity in entities)
        {
            var entityPosition = entity.Position;
            if (entityPosition.BlockX == position.BlockX && entity.Position.BlockY == position.BlockY && entityPosition.BlockZ == position.BlockZ)
            {
                return true;
            }
        }

        return false;
    }

    public Material GetBlockTypeAt(int x, int y, int z)
    {
        var chunk = GetChunkAt(x, z);
        if (chunk is null)
        {
            return Material.Air;
        }

        var blockType = chunk.GetBlockType(x, y, z);
        var material = Server.BlockRegistry.GetMaterial(blockType);

        return material;
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

    public void AddPlayer(Player player)
    {
        playerManager.Add(player);
        entityManager.Add(player);
    }

    public void AddEntity(Entity entity)
    {
        entityManager.Add(entity);
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
            if (!chunk.IsUsed)
            {
                chunkManager.UnloadChunk(chunk);
                continue;
            }

            chunk.Tick();
        }
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || (obj is World other && Equals(other));
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