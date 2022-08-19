using Minesharp.Server.Entities;

namespace Minesharp.Server.Common.Broadcast;

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