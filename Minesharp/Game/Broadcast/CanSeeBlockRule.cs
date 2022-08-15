using Minesharp.Game.Blocks;
using Minesharp.Game.Entities;

namespace Minesharp.Game.Broadcast;

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