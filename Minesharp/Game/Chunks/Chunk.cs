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