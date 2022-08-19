using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class PositionChangeProcessor : PacketProcessor<PositionChangePacket>
{
    protected override void Process(NetworkSession session, PositionChangePacket packet)
    {
        session.Player.Position = packet.Position;
        session.Player.IsGrounded = packet.IsGrounded;
    }
}