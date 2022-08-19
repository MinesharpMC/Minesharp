using Minesharp.Events.Player;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class RotationAndPositionChangeProcessor : PacketProcessor<RotationAndPositionChangePacket>
{
    protected override void Process(NetworkSession session, RotationAndPositionChangePacket packet)
    {
        var player = session.Player;
        var e = session.Player.Server.CallEvent(new PlayerMoveEvent(session.Player, new Location(player.World, player.Position), new Location(player.World, packet.Position)));

        if (e.IsCancelled)
        {
            player.SendPosition();
            return;
        }

        player.Position = !e.From.World.Equals(e.To.World) 
            ? packet.Position 
            : e.To.Position;
        
        player.Rotation = packet.Rotation;
        player.IsGrounded = packet.IsGrounded;
    }
}