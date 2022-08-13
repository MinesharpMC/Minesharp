using Minesharp.Packet.Game.Client;

namespace Minesharp.Network.Processor.Game;

public class UseItemOnProcessor : PacketProcessor<UseItemOnPacket>
{
    protected override void Process(NetworkSession session, UseItemOnPacket packet)
    {
        
    }
}