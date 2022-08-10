using Minesharp.Game;
using Minesharp.Network.Packet.Client.Play;

namespace Minesharp.Network.Processor.Play;

public class PlayerRotationProcessor : PacketProcessor<PlayerRotationPacket>
{
    protected override void Process(NetworkClient client, PlayerRotationPacket packet)
    {
        client.Player.Rotation = new Rotation
        {
            Yaw = packet.Yaw,
            Pitch = packet.Pitch
        };
    }
}