using Minesharp.Events.Player;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class PositionChangeProcessor : PacketProcessor<PositionPacket>
{
    protected override void Process(NetworkSession session, PositionPacket packet)
    {
        var player = session.Player;
        var e = session.Player.Server.CallEvent(new PlayerMoveEvent(session.Player, player.Position, packet.Position));

        if (e.IsCancelled)
        {
            player.SendPosition();
            return;
        }

        player.Position = e.To;
        player.IsGrounded = packet.IsGrounded;
    }
}