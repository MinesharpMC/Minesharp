namespace Minesharp;

public readonly struct Vector : IEquatable<Vector>
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    
    public Vector(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Vector other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(Vector left, Vector right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector left, Vector right)
    {
        return !left.Equals(right);
    }
}