using Minesharp.Server.Game.Broadcast;
using Minesharp.Server.Game.Enum;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Network.Processor.Game;

public class SwingArmProcessor : PacketProcessor<SwingArmPacket>
{
    protected override void Process(NetworkSession session, SwingArmPacket packet)
    {
        session.Player.World.Broadcast(new EntityAnimationPacket(session.Player.Id, Animation.SwingMainHand), new CanSeeEntityRule(session.Player));
    }
}