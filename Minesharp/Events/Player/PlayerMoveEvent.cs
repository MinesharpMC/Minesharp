using Minesharp.Entities;

namespace Minesharp.Events.Player;

public class PlayerMoveEvent : IEvent
{
    public IPlayer Player { get; }
    public Location From { get; set; }
    public Location To { get; set; }
    
    public bool IsCancelled { get; set; }

    public PlayerMoveEvent(IPlayer player, Location from, Location to)
    {
        Player = player;
        From = from;
        To = to;
    }
}