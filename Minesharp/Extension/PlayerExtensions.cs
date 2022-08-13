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
            X = player.Position.X,
            Y = player.Position.Y,
            Z = player.Position.Z,
            Pitch = player.Rotation.Pitch,
            Yaw = player.Rotation.Yaw
        });
    }
}