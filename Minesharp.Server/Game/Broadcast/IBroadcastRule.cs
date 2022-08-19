using Minesharp.Server.Game.Entities;

namespace Minesharp.Server.Game.Broadcast;

public interface IBroadcastRule
{
    bool IsAllowed(Player player);
}