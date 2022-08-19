using Minesharp.Server.Entities;

namespace Minesharp.Server.Common.Broadcast;

public class CanSeeEntityRule : IBroadcastRule
{
    private readonly Entity entity;

    public CanSeeEntityRule(Entity entity)
    {
        this.entity = entity;
    }

    public bool IsAllowed(Player player)
    {
        return player.CanSee(entity);
    }
}