using Minesharp.Game.Worlds;
using Minesharp.Utility;

namespace Minesharp.Game.Entities;

public sealed class Player
{
    private readonly ThreadSafeProperty<string> name = new();
    private readonly ThreadSafeProperty<World> world = new();
    private readonly ThreadSafeProperty<Position> position = new();
    private readonly ThreadSafeProperty<Rotation> rotation = new();

    public string Name
    {
        get => name.Value;
        set => name.Value = value;
    }

    public World World
    {
        get => world.Value;
        set => world.Value = value;
    }

    public Position Position
    {
        get => position.Value;
        set => position.Value = value;
    }

    public Rotation Rotation
    {
        get => rotation.Value;
        set => rotation.Value = value;
    }
}