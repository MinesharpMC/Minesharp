using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class SwingArmProcessor : PacketProcessor<SwingArmPacket>
{
    protected override void Process(NetworkSession session, SwingArmPacket packet)
    {
        var world = session.Player.World;

        var players = world.GetPlayers();
        foreach (var player in players)
        {
            var knownEntities = player.KnownEntities;
            if (knownEntities.Contains(session.Player.Id))
            {
                player.SendPacket(new EntityAnimationPacket
                {
                    EntityId = session.Player.Id,
                    Animation = Animation.SwingMainHand
                });
            }
        }
    }
}