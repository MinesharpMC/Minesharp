using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Blocks;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Chunks;

public sealed class Chunk : IEquatable<Chunk>
{
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

    public bool IsLocked => lockCount > 0;

    private int lockCount;
    private readonly List<ModifiedBlock> modifiedBlocks = new();
    
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
    
    public IEnumerable<ModifiedBlock> GetModifiedBlocks()
    {
        return modifiedBlocks;
    }

    public Material GetTypeAt(int x, int y, int z)
    {
        var chunkX = x & 0xF;
        var chunkZ = z & 0xF;
        var chunkY = y >> 4;
        
        var section = Sections.GetValueOrDefault(chunkY);
        var output = section == null
            ? Material.Air
            : (Material)section.GetType(chunkX, y, chunkZ);
                
        return output;
    }

    public void SetTypeAt(int x, int y, int z, Material material)
    {
        var chunkX = x & 0xF;
        var chunkZ = z & 0xF;
        var chunkY = y >> 4;
        
        var section = Sections.GetValueOrDefault(chunkY);
        if (section is null)
        {
            if (material == Material.Air)
            {
                return;
            }

            Sections[chunkY] = section = new ChunkSection();
        }

        var heightIndex = chunkZ * 16 + chunkX;
        if (material == Material.Air)
        {
            if (Heightmap[heightIndex] == y + 1)
            {
                Heightmap[heightIndex] = (sbyte) Sections.GetHighestTypeAt(chunkX, y, chunkZ);
            }
        }
        else
        {
            if (Heightmap[heightIndex] <= y)
            {
                Heightmap[heightIndex] = (sbyte)Math.Min(y + 1, 255);
            }
        }
        
        section.SetType(chunkX, y, chunkZ, (int)material);
        modifiedBlocks.Add(new ModifiedBlock
        {
            Position = new Position(x, y, z),
            Type = material
        });
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

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Chunk other && Equals(other);
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
        modifiedBlocks.Clear();
    }
}