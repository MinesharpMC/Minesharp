namespace Minesharp.Server.Game.Chunks;

public readonly struct ChunkKey : IEquatable<ChunkKey>
{
    public ChunkKey(int x, int z)
    {
        X = x;
        Z = z;
    }

    public int X { get; }
    public int Z { get; }

    public bool Equals(ChunkKey other)
    {
        return X == other.X && Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        return obj is ChunkKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

    public static bool operator ==(ChunkKey left, ChunkKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ChunkKey left, ChunkKey right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Z)}: {Z}";
    }

    public static ChunkKey Of(int x, int z)
    {
        return new ChunkKey(x, z);
    }
}