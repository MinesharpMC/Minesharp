namespace Minesharp.Server.Game.Schedule;

public abstract class ScheduledTask
{
    public Guid Id { get; } = Guid.NewGuid();
    public Action Task { get; init; }
    public bool CanBeRemove { get; protected set; }
    public long Ticks { get; protected set; }

    public abstract void Tick();
}