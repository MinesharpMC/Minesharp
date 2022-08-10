using Minesharp.Game;
using Minesharp.Network.Packet.Client.Play;

namespace Minesharp.Network.Processor.Play;

public class PlayerPositionProcessor : PacketProcessor<PlayerPositionPacket>
{
    protected override void Process(NetworkClient client, PlayerPositionPacket packet)
    {
        client.Player.Position = new Position
        {
            X = packet.X,
            Y = packet.Y,
            Z = packet.Z
        };
    }
}