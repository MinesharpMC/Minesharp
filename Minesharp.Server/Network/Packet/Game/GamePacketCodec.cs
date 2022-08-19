namespace Minesharp.Server.Network.Packet.Game;

public abstract class GamePacketCodec<T> : PacketCodec<T> where T : GamePacket
{
    public override Protocol Protocol => Protocol.Game;
}