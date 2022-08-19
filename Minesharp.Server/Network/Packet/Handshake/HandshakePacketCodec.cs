namespace Minesharp.Server.Network.Packet.Handshake;

public abstract class HandshakePacketCodec<T> : PacketCodec<T> where T : HandshakePacket
{
    public override Protocol Protocol => Protocol.Handshake;
}