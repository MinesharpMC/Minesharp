using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class PlayerCommandProcessor : PacketProcessor<PlayerCommandPacket>
{
    protected override void Process(NetworkSession session, PlayerCommandPacket packet)
    {
        var player = session.Player;
        switch (packet.Command)
        {
            case PlayerCommand.StartSneaking:
                player.IsSneaking = true;
                break;
            case PlayerCommand.StopSneaking:
                player.IsSneaking = false;
                break;
            case PlayerCommand.LeaveBed:
                break;
            case PlayerCommand.StartSprinting:
                player.IsSprinting = true;
                break;
            case PlayerCommand.StopSprinting:
                player.IsSprinting = false;
                break;
            case PlayerCommand.StartJumpWithHorse:
                break;
            case PlayerCommand.StopJumpWithHorse:
                break;
            case PlayerCommand.OpenHorseInventory:
                break;
            case PlayerCommand.StartFlyingWithElytra:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}