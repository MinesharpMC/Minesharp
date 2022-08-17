using System.Numerics;
using Minesharp.Common.Enum;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network.Processor.Game;

public class BlockPlaceProcessor : PacketProcessor<BlockPlacePacket>
{
    protected override void Process(NetworkSession session, BlockPlacePacket packet)
    {
        var player = session.Player;
        var block = player.World.GetBlockAt(packet.Position);
        var target = block.GetRelative(packet.Face);
        
        if (target.Type == Material.Air)
        {
            target.Type = Material.Stone;
        }

        session.SendPacket(new AckActionPacket
        {
            Sequence = packet.Sequence
        });
    }
}