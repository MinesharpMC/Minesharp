using Minesharp.Extension;

namespace Minesharp;

public readonly struct Position : IEquatable<Position>
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public int BlockX { get; }
    public int BlockY { get; }
    public int BlockZ { get; }

    public Position(int x, int y, int z) : this((double)x, y, z)
    {
    }

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
        BlockX = (int)Math.Floor(X);
        BlockY = (int)Math.Floor(Y);
        BlockZ = (int)Math.Floor(Z);
    }

    public double DistanceSquared(Position position)
    {
        return (X - position.X).Square() + (Y - position.Y).Square() + (Z - position.Z).Square();
    }

    public Position Delta(Position position)
    {
        var dx = X * 32 - position.X * 32;
        var dy = Y * 32 - position.Y * 32;
        var dz = Z * 32 - position.Z * 32;

        dx *= 128;
        dy *= 128;
        dz *= 128;

        return new Position(dx, dy, dz);
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