using Minesharp.Worlds;

namespace Minesharp;

public sealed class Location
{
    public Position Position { get; }
    public IWorld World { get; }

    public Location(IWorld world, Position position)
    {
        Position = position;
        World = world;
    }
}