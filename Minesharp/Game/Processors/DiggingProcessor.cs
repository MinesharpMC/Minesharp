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

    public void Start(Block b)
    {
        if (b == null || b.Type == Material.Air)
        {
            return;
        }
        
        if (block != null)
        {
            return;
        }

        block = b;
        ticks = 0;
        requiredTicks = (long)(((1.5 * block.GetHardness()) * Server.TickRate) + 0.5);
        block.World.BroadcastBlockDestroyStage(player, block, 0);
    }

    public void Stop()
    {
        if (block == null)
        {
            return;
        }
        
        block.World.BroadcastBlockDestroyStage(player, block, 10);
        block = null;
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
            world.BroadcastBlockDestroyStage(player, block, stage);
            return;
        }

        world.BroadcastBlockDestroyStage(player, block, 10);
        world.BroadcastBlockBreak(block, player);
        
        block.Break();
        block = null;
    }
}