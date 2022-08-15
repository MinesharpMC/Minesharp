namespace Minesharp.Game.Schedule;

public class RepeatingTask : ScheduledTask
{
    public long Delay { get; init; }

    public RepeatingTask()
    {
        CanBeRemove = false;
    }
    
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