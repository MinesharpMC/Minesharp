namespace Minesharp.Game;

public readonly struct Position
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }

    public int BlockX => (int)Math.Floor(X);
    public int BlockY => (int)Math.Floor(Y);
    public int BlockZ => (int)Math.Floor(Z);

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}, {nameof(BlockX)}: {BlockX}, {nameof(BlockY)}: {BlockY}, {nameof(BlockZ)}: {BlockZ}";
    }
}