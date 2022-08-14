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

        if (block.Type == Material.Air)
        {
            return;
        }

        switch (packet.Status)
        {
            case ActionStatus.StartDigging:
                if (player.GameMode == GameMode.Creative)
                {
                    block.Type = Material.Air;
                }
                break;
            case ActionStatus.FinishDigging:
                block.Type = Material.Air;
                break;
            case ActionStatus.CancelDigging:
                break;
            case ActionStatus.DropItemStack:
                break;
            case ActionStatus.DropItem:
                break;
            case ActionStatus.ShootArrow:
                break;
            case ActionStatus.SwapItem:
                break;
        }
        
        session.SendPacket(new AckBlockChangePacket
        {
            Sequence = packet.Sequence
        });
    }
}