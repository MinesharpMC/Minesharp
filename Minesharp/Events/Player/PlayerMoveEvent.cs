using Minesharp.Entities;

namespace Minesharp.Events.Player;

/// <summary>
/// Event called when a player move
/// </summary>
public class PlayerMoveEvent : IEvent
{
    /// <summary>
    /// Player moving
    /// </summary>
    public IPlayer Player { get; }
    
    /// <summary>
    /// Position where the player is
    /// </summary>
    public Position From { get; set; }
    
    /// <summary>
    /// Position where he want to go
    /// </summary>
    public Position To { get; set; }
    
    /// <summary>
    /// Define if this event should be cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    public PlayerMoveEvent(IPlayer player, Position from, Position to)
    {
        Player = player;
        From = from;
        To = to;
    }
}