using Minesharp.Server.Blocks;
using Minesharp.Server.Entities;

namespace Minesharp.Server.Common.Broadcast;

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