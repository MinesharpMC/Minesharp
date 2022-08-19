using Minesharp.Server.Game.Blocks;
using Minesharp.Server.Game.Entities;

namespace Minesharp.Server.Game.Broadcast;

public class CanSeeBlockRule : IBroadcastRule
{
    private readonly Block block;

    public CanSeeBlockRule(Block block)
    {
        this.block = block;
    }

    public bool IsAllowed(Player player)
    {
        return player.CanSee(block);
    }
}