using Minesharp.Game.Blocks;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class WorldExtensions
{
    public static void BroadcastBlockBreakAnimation(this World world, Player player, Block block, byte stage)
    {
        world.Broadcast(new BlockBreakAnimationPacket
        {
            EntityId = player.Id,
            Position = block.Position,
            Stage = stage
        }, new CanSeeBlockRule(block), new ExceptPlayerRule(player));
    }
}