using System.Collections.Concurrent;

namespace Minesharp.Game;

public sealed class Scheduler
{
    private readonly ConcurrentQueue<Action> tasks = new();
    
    public void Schedule(Action task)
    {
        tasks.Enqueue(task);
    }

    public void Tick()
    {
        while (tasks.TryDequeue(out var task))
        {
            task();
        }
    }
}