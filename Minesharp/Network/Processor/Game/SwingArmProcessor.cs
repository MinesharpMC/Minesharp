using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game.Broadcast;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class SwingArmProcessor : PacketProcessor<SwingArmPacket>
{
    protected override void Process(NetworkSession session, SwingArmPacket packet)
    {
        session.Player.World.Broadcast(new EntityAnimationPacket(session.Player.Id, Animation.SwingMainHand), new CanSeeEntityRule(session.Player));
    }
}