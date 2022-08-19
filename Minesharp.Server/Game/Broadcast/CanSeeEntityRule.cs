using Minesharp.Server.Game.Entities;

namespace Minesharp.Server.Game.Broadcast;

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