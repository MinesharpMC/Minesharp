namespace Minesharp.Game.Worlds;

public class World
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public Server Server { get; init; }
    public long Seed { get; init; }
}