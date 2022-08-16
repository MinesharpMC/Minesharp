using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Network.Processor.Game;

public class ActionProcessor : PacketProcessor<ActionPacket>
{
    protected override void Process(NetworkSession session, ActionPacket packet)
    {
        var player = session.Player;
        var world = player.World;
        var block = world.GetBlockAt(packet.Position);

        switch (packet.Status)
        {
            case ActionStatus.Digging:
                player.StartDigging(block);
                break;
            case ActionStatus.CancelDigging:
                player.StopDigging();
                break;
            case ActionStatus.FinishDigging:
                break;
        }
    }
}