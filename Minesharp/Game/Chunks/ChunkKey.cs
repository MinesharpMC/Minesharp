namespace Minesharp.Game.Chunks;

public class ChunkKey : IEquatable<ChunkKey>
{
    private readonly int x;
    private readonly int z;
    private readonly long id;

    public ChunkKey(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.id = x & 0xffffffffL | (z & 0xffffffffL) << 32;
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

    public long GetId()
    {
        return id;
    }

    public bool Equals(ChunkKey other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return id == other.id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ChunkKey)obj);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
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