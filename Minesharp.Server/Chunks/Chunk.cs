using Minesharp.Blocks;
using Minesharp.Chunks;
using Minesharp.Entities;
using Minesharp.Server.Blocks;
using Minesharp.Server.Extension;
using Minesharp.Server.Worlds;
using Minesharp.Worlds;

namespace Minesharp.Server.Chunks;

public sealed class Chunk : IEquatable<Chunk>, IChunk
{
    private int lockCount;
    private readonly List<BlockChange> changes = new();

    public Chunk(ChunkKey key, World world)
    {
        Key = key;
        World = world;
    }

    public ChunkKey Key { get; }

    public Dictionary<int, ChunkSection> Sections { get; set; }
    public sbyte[] Heightmap { get; set; }
    public World World { get; }

    public bool IsUsed => lockCount > 0;
    public int X => Key.X;
    public int Z => Key.Z;

    public IWorld GetWorld()
    {
        return World;
    }

    public IEnumerable<IEntity> GetEntities()
    {
        var output = new List<IEntity>();

        var entities = World.GetEntities();
        foreach (var entity in entities)
        {
            var chunk = World.GetChunkAt(entity.Position);
            if (chunk == this)
            {
                output.Add(entity);
            }
        }

        return output;
    }

    public IEnumerable<IPlayer> GetPlayers()
    {
        var output = new List<IPlayer>();

        var players = World.GetPlayers();
        foreach (var player in players)
        {
            var chunk = World.GetChunkAt(player.Position);
            if (chunk == this)
            {
                output.Add(player);
            }
        }

        return output;
    }

    public IBlock GetBlock(int x, int y, int z)
    {
        return GetBlockAt(x, y, z);
    }

    public Block GetBlockAt(int x, int y, int z)
    {
        return World.GetBlockAt(x, y, z);
    }

    public bool Equals(Chunk other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Key.Equals(other.Key);
    }

    public void AddLock()
    {
        lockCount++;
    }

    public void RemoveLock()
    {
        if (lockCount > 0)
        {
            lockCount--;
        }
    }

    public IList<BlockChange> GetChanges()
    {
        return changes;
    }

    public int GetBlockType(int x, int y, int z)
    {
        var chunkX = x & 0xF;
        var chunkZ = z & 0xF;
        var chunkY = y >> 4;

        var section = Sections.GetValueOrDefault(chunkY);
        return section == null
            ? 0
            : section.GetType(chunkX, y, chunkZ);
    }

    public void SetBlockType(int x, int y, int z, int type)
    {
        var chunkX = x & 0xF;
        var chunkZ = z & 0xF;
        var chunkY = y >> 4;

        var section = Sections.GetValueOrDefault(chunkY);
        if (section is null)
        {
            if (type == 0)
            {
                return;
            }

            Sections[chunkY] = section = new ChunkSection();
        }

        var heightIndex = chunkZ * 16 + chunkX;
        if (type == 0)
        {
            if (Heightmap[heightIndex] == y + 1)
            {
                Heightmap[heightIndex] = (sbyte)Sections.GetHighestTypeAt(chunkX, y, chunkZ);
            }
        }
        else
        {
            if (Heightmap[heightIndex] <= y)
            {
                Heightmap[heightIndex] = (sbyte)Math.Min(y + 1, 255);
            }
        }

        section.SetType(chunkX, y, chunkZ, type);

        changes.Add(new BlockChange
        {
            Position = new Position(x, y, z),
            BlockType = type
        });
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || (obj is Chunk other && Equals(other));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key);
    }

    public static bool operator ==(Chunk left, Chunk right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Chunk left, Chunk right)
    {
        return !Equals(left, right);
    }

    public void Tick()
    {
        changes.Clear();
    }
}