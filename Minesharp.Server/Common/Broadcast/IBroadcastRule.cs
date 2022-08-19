using Minesharp.Server.Entities;

namespace Minesharp.Server.Common.Broadcast;

public interface IBroadcastRule
{
    bool IsAllowed(Player player);
}