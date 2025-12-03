using Minesharp.Events.Player;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class RotationAndPositionChangeProcessor : PacketProcessor<RotationAndPositionChangePacket>
{
    protected override void Process(NetworkSession session, RotationAndPositionChangePacket packet)
    {
        var player = session.Player;
        var e = session.Player.Server.Publish(new PlayerMoveEvent(session.Player, player.Position, packet.Position));

        if (e.IsCancelled)
        {
            player.SendPosition();
            return;
        }

        player.Position = e.To;
        player.Rotation = packet.Rotation;
        player.IsGrounded = packet.IsGrounded;
    }
}