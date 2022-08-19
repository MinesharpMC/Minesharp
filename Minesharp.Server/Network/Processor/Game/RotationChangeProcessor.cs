using Minesharp.Server.Network.Packet.Game.Client;

namespace Minesharp.Server.Network.Processor.Game;

public class RotationChangeProcessor : PacketProcessor<RotationChangePacket>
{
    protected override void Process(NetworkSession session, RotationChangePacket packet)
    {
        session.Player.Rotation = packet.Rotation;
    }
}