using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Blocks;
using Minesharp.Game.Worlds;

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
    public ChunkSection[] Sections { get; init; }
    public sbyte[] Heightmap { get; init; }
    public World World { get; }

    public Queue<ModifiedBlock> ModifiedBlocks { get; } = new();

    public Material GetTypeAt(int x, int y, int z)
    {
        var section = Sections.GetSection(y);
        var output = section == null
            ? Material.Air
            : (Material)section.GetType(x, y, z);
                
        return output;
    }

    public void SetTypeAt(int x, int y, int z, Material material)
    {
        ModifiedBlocks.Enqueue(new ModifiedBlock
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

        return Key.Equals(other.Key) && Equals(World, other.World);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj) || obj is Chunk other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, World);
    }

    public static bool operator ==(Chunk left, Chunk right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Chunk left, Chunk right)
    {
        return !Equals(left, right);
    }
}