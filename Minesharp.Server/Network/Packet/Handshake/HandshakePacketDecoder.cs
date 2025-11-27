namespace Minesharp.Server.Network.Packet.Handshake;

public abstract class HandshakePacketDecoder<T> : PacketDecoder<T> where T : HandshakePacket
{
    public override Protocol Protocol => Protocol.Handshake;
}