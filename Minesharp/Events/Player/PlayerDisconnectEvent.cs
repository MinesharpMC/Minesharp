using Minesharp.Entities;

namespace Minesharp.Events.Player;

/// <summary>
/// Event called when a player disconnect
/// </summary>
public class PlayerDisconnectEvent : IEvent
{
    public PlayerDisconnectEvent(IPlayer player)
    {
        Player = player;
    }

    /// <summary>
    /// Player who disconnected
    /// </summary>
    public IPlayer Player { get; }
}