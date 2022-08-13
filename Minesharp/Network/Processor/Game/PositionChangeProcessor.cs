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
        var currentPosition = new Position(packet.X, packet.Y, packet.Z);
        
        var previousChunk = player.World.GetChunkAt(previousPosition);
        var currentChunk = player.World.GetChunkAt(currentPosition);

        if (previousChunk != currentChunk)
        {
            player.SendPacket(new SetCenterChunkPacket
            {
                ChunkX = currentChunk.X,
                ChunkZ = currentChunk.Z
            });
        }
        
        session.Player.Position = new Position(packet.X, packet.Y, packet.Z);
    }
}