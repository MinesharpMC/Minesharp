using Minesharp.Packet.Game.Client;
using Serilog;

namespace Minesharp.Network.Processor.Game;

public class ActionProcessor : PacketProcessor<ActionPacket>
{
    protected override void Process(NetworkSession session, ActionPacket packet)
    {
        
    }
}