namespace Minesharp.Game.Schedule;

public class SimpleTask : ScheduledTask
{
    public long Delay { get; init; }

    public override void Tick()
    {
        if (Delay == Ticks)
        {
            Task();
            Ticks = 0;
            CanBeRemove = true;
            return;
        }

        Ticks++;
    }
}