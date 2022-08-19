using Minesharp.Worlds;

namespace Minesharp;

public sealed class Location
{
    public Position Position { get; init; }
    public IWorld World { get; init; }
}