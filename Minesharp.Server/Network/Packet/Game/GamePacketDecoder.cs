namespace Minesharp.Server.Network.Packet.Game;

public abstract class GamePacketDecoder<T> : PacketDecoder<T> where T : GamePacket
{
    public override Protocol Protocol => Protocol.Game;
}