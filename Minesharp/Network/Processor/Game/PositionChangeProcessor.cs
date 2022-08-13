using Minesharp.Common;
using Minesharp.Game;
using Minesharp.Packet.Game.Client;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Network.Processor.Game;

public class PositionChangeProcessor : PacketProcessor<PositionChangePacket>
{
    protected override void Process(NetworkSession session, PositionChangePacket packet)
    {
        var player = session.Player;
        
        var previousPosition = player.Position;
        var currentPosition = player.Position = packet.Position;
        
        if (previousPosition.BlockX != currentPosition.BlockX || previousPosition.BlockZ != currentPosition.BlockZ)
        {
            var currentChunk = player.World.GetChunkAt(currentPosition);
            var previousChunk = player.World.GetChunkAt(previousPosition);

            if (currentChunk != previousChunk)
            {
                session.SendPacket(new SetCenterChunkPacket
                {
                    ChunkX = currentChunk.X,
                    ChunkZ = currentChunk.Z
                });
            }
        }
        
    }
}