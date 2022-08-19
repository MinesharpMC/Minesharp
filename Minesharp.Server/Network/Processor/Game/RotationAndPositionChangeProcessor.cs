using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class RotationAndPositionChangeProcessor : PacketProcessor<RotationAndPositionChangePacket>
{
    protected override void Process(NetworkSession session, RotationAndPositionChangePacket packet)
    {
        session.Player.Position = packet.Position;
        session.Player.Rotation = packet.Rotation;
        session.Player.IsGrounded = packet.IsGrounded;
    }
}