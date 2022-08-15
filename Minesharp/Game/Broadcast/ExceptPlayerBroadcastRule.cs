using Minesharp.Game.Entities;

namespace Minesharp.Game.Broadcast;

public class ExceptPlayerBroadcastRule : IBroadcastRule
{
    private readonly Player player;

    public ExceptPlayerBroadcastRule(Player player)
    {
        this.player = player;
    }

    public bool IsAllowed(Player other)
    {
        return this.player != other;
    }
}