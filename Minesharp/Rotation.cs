namespace Minesharp;

public readonly struct Rotation : IEquatable<Rotation>
{
    public float Pitch { get; }
    public float Yaw { get; }

    public Rotation(float pitch, float yaw)
    {
        Pitch = pitch;
        Yaw = yaw;
    }

    public bool Equals(Rotation other)
    {
        return Pitch.Equals(other.Pitch) && Yaw.Equals(other.Yaw);
    }

    public override bool Equals(object obj)
    {
        return obj is Rotation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pitch, Yaw);
    }

    public static bool operator ==(Rotation left, Rotation right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rotation left, Rotation right)
    {
        return !left.Equals(right);
    }
    
    public Vector ToDirection()
    {
        var yawRad = Math.PI * Yaw / 180.0;
        var pitchRad = Math.PI * Pitch / 180.0;
        
        var x = -Math.Sin(yawRad) * Math.Cos(pitchRad);
        var y = -Math.Sin(pitchRad);
        var z =  Math.Cos(yawRad) * Math.Cos(pitchRad);

        return new Vector(x, y, z).Normalized();
    }

    public override string ToString()
    {
        return $"{nameof(Pitch)}: {Pitch}, {nameof(Yaw)}: {Yaw}";
    }
}