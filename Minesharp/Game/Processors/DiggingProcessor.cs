using System.Net;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Blocks;
using Minesharp.Game.Broadcast;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Game.Processors;

public class DiggingProcessor
{
    private readonly Player player;

    private Block block;
    private long ticks;
    private long requiredTicks;


    public DiggingProcessor(Player player)
    {
        this.player = player;
    }

    public void Dig(Block b)
    {
        if (block == b)
        {
            return;
        }

        if (b == null)
        {
            block.World.BroadcastBlockBreakAnimation(player, block, 10);
            block = null;
            return;
        }
        
        block = b;
        ticks = 0;
        requiredTicks = (long)(((1.5 * block.GetHardness()) * Server.TickRate) + 0.5);
        block.World.BroadcastBlockBreakAnimation(player, block, 0);
    }

    public void Tick()
    {
        if (block == null)
        {
            return;
        }
        
        var world = block.World;
        if (++ticks <= requiredTicks)
        {
            var stage = (byte)(10.0 * (ticks - 1) / requiredTicks);
            world.BroadcastBlockBreakAnimation(player, block, stage);
            return;
        }

        block.Break();
        Dig(null);
    }
}