using Minesharp.Events.Block;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Worlds;

namespace Minesharp.Server.Network.Processor.Game;

public class PlayerActionProcessor : PacketProcessor<PlayerActionPacket>
{
    protected override void Process(NetworkSession session, PlayerActionPacket packet)
    {
        var player = session.Player;
        var world = player.World;
        var block = world.GetBlockAt(packet.Position);

        switch (packet.Action)
        {
            case PlayerAction.StartDigging:
                player.Breaking = block;
                break;
            case PlayerAction.StopDigging:
                player.Breaking = null;
                break;
            case PlayerAction.FinishDigging:
                if (block != player.Breaking)
                {
                    return;
                }

                var e = player.Server.Publish(new BlockBreakEvent(block, player));
                if (!e.IsCancelled)
                {
                    var drops = block.GetDrops();
                    
                    block.BreakBy(player);
                    
                    foreach (var drop in drops)
                    {
                        block.World.DropItem(block.Position, drop);
                    }
                }

                player.Breaking = null;
                break;
        }

        player.SendAckBlockChange(packet.Sequence);
    }
}