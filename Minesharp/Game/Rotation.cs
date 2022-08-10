namespace Minesharp.Game;

public readonly struct Rotation
{
    public float Pitch { get; init; }
    public float Yaw { get; init; }

    public override string ToString()
    {
        return $"{nameof(Pitch)}: {Pitch}, {nameof(Yaw)}: {Yaw}";
    }
}