namespace Minesharp.Server.Game.Schedule;

public class RepeatingTask : ScheduledTask
{
    public RepeatingTask()
    {
        CanBeRemove = false;
    }

    public long Delay { get; init; }

    public override void Tick()
    {
        if (Delay == Ticks)
        {
            Task();
            Ticks = 0;
            return;
        }

        Ticks++;
    }
}