using Minesharp.Common;
using Minesharp.Extension;
using Minesharp.Game.Blocks;
using Minesharp.Game.Worlds;

namespace Minesharp.Game.Chunks;

public sealed class Chunk : IEquatable<Chunk>
{
    private int lockCount;
    private readonly List<BlockChange> changes = new();

    public Chunk(ChunkKey key, World world)
    {
        Key = key;
        World = world;
    }

    public ChunkKey Key { get; }
    public int X => Key.X;
    public int Z => Key.Z;
    public Dictionary<int, ChunkSection> Sections { get; set; }
    public sbyte[] Heightmap { get; set; }
    public World World { get; }

    public bool IsUsed => lockCount > 0;

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