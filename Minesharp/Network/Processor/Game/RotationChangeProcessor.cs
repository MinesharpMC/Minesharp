using Minesharp.Game;
using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class RotationChangeProcessor : PacketProcessor<RotationChangePacket>
{
    protected override void Process(NetworkSession session, RotationChangePacket packet)
    {
        session.Player.Position = new Position(packet.X, packet.Y, packet.Z);
        session.Player.Rotation = new Rotation(packet.Pitch, packet.Yaw);
    }
}