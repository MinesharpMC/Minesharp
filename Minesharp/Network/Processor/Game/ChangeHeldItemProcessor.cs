using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class ChangeHeldItemProcessor : PacketProcessor<ChangeHeldItemPacket>
{
    protected override void Process(NetworkSession session, ChangeHeldItemPacket packet)
    {
        var world = session.Player.World;
        var players = world.GetPlayers();
        foreach (var player in players)
        {
            if (!player.CanSee(session.Player))
            {
                continue;
            }
            
            player.SendPacket(new EquipmentPacket
            {
                EntityId = session.Player.Id,
                Slot = EquipmentSlot.MainHand,
                Item = new ItemInfo
                {
                    Amount = 1,
                    Material = Material.Stone
                }
            });
        }
    }
}