using Minesharp.Server.Game.Entities;

namespace Minesharp.Server.Game.Broadcast;

public class ExceptPlayerRule : IBroadcastRule
{
    private readonly Player player;

    public ExceptPlayerRule(Player player)
    {
        this.player = player;
    }

    public bool IsAllowed(Player other)
    {
        if (player == null)
        {
            return true;
        }

        return player != other;
    }
}