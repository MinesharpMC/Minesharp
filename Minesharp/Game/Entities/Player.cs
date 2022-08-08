using Minesharp.Utility;

namespace Minesharp.Game.Entities;

public class Player
{
    private readonly LockedProperty<string> name = new();
    private readonly LockedProperty<Position> position = new();

    public Guid Id { get; init; }
    
    public string Name
    {
        get => name.Value;
        set => name.Value = value;
    }

    public Position Position
    {
        get => position.Value;
        set => position.Value = value;
    }
}