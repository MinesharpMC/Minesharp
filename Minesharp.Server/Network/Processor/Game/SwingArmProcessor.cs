using Minesharp.Server.Common.Broadcast;
using Minesharp.Server.Common.Enum;
using Minesharp.Server.Network.Packet.Game.Client;
using Minesharp.Server.Network.Packet.Game.Server;

namespace Minesharp.Server.Network.Processor.Game;

public class SwingArmProcessor : PacketProcessor<SwingArmPacket>
{
    protected override void Process(NetworkSession session, SwingArmPacket packet)
    {
        var animation = packet.Hand == Hand.MainHand ? Animation.SwingMainHand : Animation.SwingOffHand;
        
        session.Player.World.Broadcast(new EntityAnimationPacket(session.Player.Id, animation),new ExceptPlayerRule(session.Player), new CanSeeEntityRule(session.Player));
    }
}