using Minesharp.Events.Block;
using Minesharp.Server.Game.Enum;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;

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

                var e = player.Server.CallEvent(new BlockBreakEvent(block));
                if (!e.IsCancelled)
                {
                    block.BreakBy(player);
                }
                
                player.Breaking = null;
                break;
        }

        player.SendPacket(new AckBlockChangePacket(packet.Sequence));
    }
}