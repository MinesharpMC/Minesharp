using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;
using Serilog;

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
                if (player.GameMode == GameMode.Creative)
                {
                    block.Type = Material.Air;
                    return;
                }
                player.Digging = block;
                break;
            case PlayerAction.StopDigging:
                player.Digging = null;
                break;
        }
    }
}