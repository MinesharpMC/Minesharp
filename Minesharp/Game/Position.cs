namespace Minesharp.Game;

public readonly struct Position
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }

    public int GetBlockX()
    {
        return (int)Math.Floor(X);
    }

    public int GetBlockZ()
    {
        return (int)Math.Floor(Y);
    }

    public int GetBlockY()
    {
        return (int)Math.Floor(Z);
    }
}