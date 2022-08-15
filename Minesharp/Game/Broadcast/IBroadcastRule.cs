using Minesharp.Game.Entities;

namespace Minesharp.Game.Broadcast;

public interface IBroadcastRule
{
    bool IsAllowed(Player player);
}