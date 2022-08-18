using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

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
                player.Breaking.BreakBy(player);
                player.Breaking = null;
                break;
        }
    }
}