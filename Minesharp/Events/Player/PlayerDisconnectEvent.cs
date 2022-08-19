using Minesharp.Entities;

namespace Minesharp.Events.Player;

public class PlayerDisconnectEvent : IEvent
{
    public PlayerDisconnectEvent(IPlayer player)
    {
        Player = player;
    }

    public IPlayer Player { get; }
}