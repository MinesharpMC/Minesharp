using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class RotationAndPositionChangeProcessor : PacketProcessor<RotationAndPositionChangePacket>
{
    protected override void Process(NetworkSession session, RotationAndPositionChangePacket packet)
    {
        session.Player.Position = packet.Position;
        session.Player.Rotation = packet.Rotation;
    }
}