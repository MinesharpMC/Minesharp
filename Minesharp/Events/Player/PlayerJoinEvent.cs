using Minesharp.Entities;

namespace Minesharp.Events.Player;

public class PlayerJoinEvent : IEvent
{
    public IPlayer Player { get; init; }
    public string Message { get; set; }
    
    public bool IsCancelled { get; set; }
}