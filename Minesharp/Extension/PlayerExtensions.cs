using Minesharp.Game;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Extension;

public static class PlayerExtensions
{
    public static void SendPosition(this Player player)
    {
        player.SendPacket(new SyncPositionPacket
        {
            Position = player.Position,
            Rotation = player.Rotation
        });
    }
}