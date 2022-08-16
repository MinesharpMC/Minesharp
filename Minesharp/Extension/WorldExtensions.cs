using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class WorldExtensions
{
    public static void BroadcastBlockDestroyStage(this World world, Player player, Block block, byte stage)
    {
        world.Broadcast(new BlockBreakAnimationPacket
        {
            EntityId = player.Id,
            Position = block.Position,
            Stage = stage
        }, new CanSeeBlockRule(block), new ExceptPlayerRule(player));
    }

    public static void BroadcastBlockBreak(this World world, Block block, Player breaker = null)
    {
        world.BroadcastEffect(block.Position, Effect.BlockBreak, (int)block.Type, radius: 16, ignore: breaker);
    }
    
    public static void BroadcastEffect(this World world, Position position, Effect effect, int data, int radius = 0, Player ignore = null)
    {
        var rules = new List<IBroadcastRule>();
        if (radius > 0)
        {
            rules.Add(new InRadiusRule(position, radius));
        }

        if (ignore is not null)
        {
            rules.Add(new ExceptPlayerRule(ignore));
        }
        
        world.Broadcast(new PlayEffectPacket
        {
            Effect = effect,
            Position = position,
            Data = data,
            IgnoreDistance = false
        }, rules.ToArray());
    }
}