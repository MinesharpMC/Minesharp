using Minesharp.Entities;

namespace Minesharp.Events.Player;

/// <summary>
///     Event called when a player join the game
/// </summary>
public class PlayerJoinEvent : IEvent
{
    public PlayerJoinEvent(IPlayer player)
    {
        Player = player;
    }

    /// <summary>
    ///     Player who joined
    /// </summary>
    public IPlayer Player { get; init; }

    /// <summary>
    ///     Message to broadcast
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///     Define if event should be cancelled or not
    /// </summary>
    public bool IsCancelled { get; set; }
}