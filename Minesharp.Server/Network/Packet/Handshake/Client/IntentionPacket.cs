using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Handshake.Client;

public sealed class IntentionPacket : HandshakePacket
{
    /// <summary>
    ///     Protocol version numbers (currently 759 in Minecraft 1.19).
    /// </summary>
    public int ProtocolVersion { get; init; }

    /// <summary>
    ///     Hostname or IP, e.g. localhost or 127.0.0.1, that was used to connect.
    ///     The Notchian server does not use this information
    /// </summary>
    public string Host { get; init; }

    /// <summary>
    ///     Default is 25565.
    ///     The Notchian server does not use this information
    /// </summary>
    public ushort Port { get; init; }

    /// <summary>
    ///     Request next state 1 for Status, 2 for Login.
    /// </summary>
    public Protocol RequestedProtocol { get; init; }
}

public sealed class IntentionPacketCodec : HandshakePacketDecoder<IntentionPacket>
{
    public override int PacketId => 0x0;

    protected override IntentionPacket Decode(IByteBuffer buffer)
    {
        var protocolVersion = buffer.ReadVarInt();
        var host = buffer.ReadString();
        var port = buffer.ReadUnsignedShort();
        var requestedProtocol = buffer.ReadVarIntEnum<Protocol>();

        return new IntentionPacket
        {
            ProtocolVersion = protocolVersion,
            Host = host,
            Port = port,
            RequestedProtocol = requestedProtocol
        };
    }
}