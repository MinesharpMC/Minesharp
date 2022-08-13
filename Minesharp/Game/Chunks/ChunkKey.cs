namespace Minesharp.Game.Chunks;

public sealed class ChunkKey : IEquatable<ChunkKey>
{
    public ChunkKey(int x, int z)
    {
        X = x;
        Z = z;
        Id = (x & 0xffffffffL) | ((z & 0xffffffffL) << 32);
    }

    public int X { get; }
    public int Z { get; }
    public long Id { get; }

    public bool Equals(ChunkKey other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ChunkKey)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(ChunkKey left, ChunkKey right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ChunkKey left, ChunkKey right)
    {
        return !Equals(left, right);
    }

    public static ChunkKey Create(int x, int z)
    {
        return new ChunkKey(x, z);
    }
}