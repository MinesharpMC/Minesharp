using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class RotationChangeProcessor : PacketProcessor<RotationChangePacket>
{
    protected override void Process(NetworkSession session, RotationChangePacket packet)
    {
        session.Player.Position = packet.Position;
        session.Player.Rotation = packet.Rotation;
    }
}