using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class SwingArmProcessor : PacketProcessor<SwingArmPacket>
{
    protected override void Process(NetworkSession session, SwingArmPacket packet)
    {
        var world = session.Player.World;

        var players = world.GetPlayers();
        foreach (var player in players)
        {
            if (!player.Known(session.Player))
            {
                continue;
            }
            
            player.SendEntityAnimation(session.Player, Animation.SwingMainHand);
        }
    }
}