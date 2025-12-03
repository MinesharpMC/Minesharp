using Minesharp.Server.Network.Packet.Game.Client;
using Serilog;

namespace Minesharp.Server.Network.Processor.Game;

public class CloseWindowProcessor : PacketProcessor<CloseWindowPacket>
{
    protected override void Process(NetworkSession session, CloseWindowPacket packet)
    {
        if (session.Player.OpenedInventory == null)
            return;

        if (session.Player.OpenedInventory.Id != packet.WindowId)
            return;
        
        session.Player.OpenedInventory = null;
        
        Log.Information("Player {Name} closed inventory", session.Player.Username);
    }
}