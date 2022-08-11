using Minesharp.Game;
using Minesharp.Network.Packet.Client.Play;

namespace Minesharp.Network.Processor.Play;

public class PlayerPositionProcessor : PacketProcessor<PlayerPositionPacket>
{
    protected override void Process(NetworkSession session, PlayerPositionPacket packet)
    {
        session.Player.Position = new Position
        {
            X = packet.X,
            Y = packet.Y,
            Z = packet.Z
        };
    }
}