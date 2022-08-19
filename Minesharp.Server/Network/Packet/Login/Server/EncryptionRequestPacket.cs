using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class EncryptionRequestPacket : LoginPacket
{
    /// <summary>
    ///     Appears to be empty.
    /// </summary>
    public string ServerId { get; init; }

    /// <summary>
    ///     The server's public key, in bytes.
    /// </summary>
    public byte[] PublicKey { get; init; }

    /// <summary>
    ///     A sequence of random bytes generated by the server.
    /// </summary>
    public byte[] Token { get; init; }
}

public sealed class EncryptionRequestPacketCodec : LoginPacketCodec<EncryptionRequestPacket>
{
    public override int PacketId => 0x01;

    protected override EncryptionRequestPacket Decode(IByteBuffer buffer)
    {
        var serverId = buffer.ReadString();
        var publicKey = buffer.ReadByteArray();
        var token = buffer.ReadByteArray();

        return new EncryptionRequestPacket
        {
            ServerId = serverId,
            PublicKey = publicKey,
            Token = token
        };
    }

    protected override void Encode(EncryptionRequestPacket packet, IByteBuffer buffer)
    {
        buffer.WriteString(packet.ServerId);
        buffer.WriteByteArray(packet.PublicKey);
        buffer.WriteByteArray(packet.Token);
    }
}