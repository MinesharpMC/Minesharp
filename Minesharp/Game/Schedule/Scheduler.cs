using System.Collections.Concurrent;

namespace Minesharp.Game.Schedule;

public sealed class Scheduler
{
    private readonly ConcurrentDictionary<Guid, ScheduledTask> tasks = new();
    
    public void ScheduleRepeatingTask(Action action, long delay = 20L)
    {
        var task = new RepeatingTask
        {
            Task = action,
            Delay = delay
        };

        tasks[task.Id] = task;
    }
    
    public void ScheduleTask(Action action, long delay = 20L)
    {
        var task = new SimpleTask
        {
            Task = action,
            Delay = delay
        };

        tasks[task.Id] = task;
    }

    public void Cancel(Guid taskId)
    {
        tasks.Remove(taskId, out _);
    }

    public void Tick()
    {
        foreach (var task in tasks.Values)
        {
            task.Tick();
            if (task.CanBeRemove)
            {
                tasks.Remove(task.Id, out _);
            }
        }
    }
}
