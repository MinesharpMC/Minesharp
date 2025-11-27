namespace Minesharp.Server.Network.Packet.Game;

public abstract class GamePacketEncoder<T> : PacketEncoder<T> where T : GamePacket
{
    public override Protocol Protocol => Protocol.Game;
}