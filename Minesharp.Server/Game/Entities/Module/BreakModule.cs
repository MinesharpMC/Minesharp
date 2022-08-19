using Minesharp.Server.Game.Blocks;
using Minesharp.Server.Game.Broadcast;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Game.Entities.Module;

public class BreakModule
{
    private Block block;

    private long ticks;
    private long ticksRequired;

    private readonly Player player;

    public BreakModule(Player player)
    {
        this.player = player;
    }

    public Block Block
    {
        get => block;
        set
        {
            if (block == value)
            {
                return;
            }

            if (value == null)
            {
                block.World.Broadcast(new BlockBreakStageUpdatePacket(player.Id, block.Position, 10), new IBroadcastRule[]
                {
                    new CanSeeBlockRule(block),
                    new InRadiusRule(block.Position, 16),
                    new ExceptPlayerRule(player)
                });
            }

            block = value;
            ticks = 0L;
            ticksRequired = block == null ? 0L : (long)(1.5 * block.Type.Hardness * GameServer.TickRate);
        }
    }

    public void Tick()
    {
        if (block == null)
        {
            return;
        }

        if (ticks++ <= ticksRequired)
        {
            var stage = (byte)((10.0 * ticks - 1) / ticksRequired);
            block.World.Broadcast(new BlockBreakStageUpdatePacket(player.Id, block.Position, (byte)(stage - 1)), new IBroadcastRule[]
            {
                new CanSeeBlockRule(block),
                new InRadiusRule(block.Position, 16),
                new ExceptPlayerRule(player)
            });
        }
    }

    public void Update()
    {
    }
}