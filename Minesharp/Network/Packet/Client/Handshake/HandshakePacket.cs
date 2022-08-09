using DotNetty.Buffers;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Client.Handshake;

public class HandshakePacket : ClientPacket
{
    public int Protocol { get; init; }
    public string Host { get; init; }
    public ushort Port { get; init; }
    public int NextState { get; init; }

    public override string ToString()
    {
        return $"{nameof(Protocol)}: {Protocol}, {nameof(Host)}: {Host}, {nameof(Port)}: {Port}, {nameof(NextState)}: {NextState}";
    }
}

public class HandshakeCreator : PacketCreator<HandshakePacket>
{
    public override int PacketId => 0x0;
    public override NetworkProtocol Protocol => NetworkProtocol.Handshake;

    protected override HandshakePacket CreatePacket(IByteBuffer buffer)
    {
        var protocol = buffer.ReadVarInt();
        var host = buffer.ReadString();
        var port = buffer.ReadUnsignedShort();
        var nextState = buffer.ReadVarInt();

        return new HandshakePacket
        {
            Protocol = protocol,
            Host = host,
            Port = port,
            NextState = nextState
        };
    }
}