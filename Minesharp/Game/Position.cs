namespace Minesharp.Game;

public readonly struct Position : IEquatable<Position>
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public BlockPosition ToBlockPosition()
    {
        return new BlockPosition((int)X, (int)Y, (int)Z);
    }

    public bool Equals(Position other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object obj)
    {
        return obj is Position other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}";
    }
}