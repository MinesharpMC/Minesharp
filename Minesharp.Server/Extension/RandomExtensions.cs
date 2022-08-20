namespace Minesharp.Server.Extension;

public static class RandomExtensions
{
    public static double NextDouble(this Random random, double maximum)
    {
        return random.NextDouble() * (maximum);
    }

    public static bool NextBoolean(this Random random)
    {
        return random.Next(0, 2) == 1;
    }
}