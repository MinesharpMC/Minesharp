using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Handshake;

public abstract class HandshakePacketEncoder<T> : PacketEncoder<T> where T : HandshakePacket
{
    public override Protocol Protocol => Protocol.Handshake;
}