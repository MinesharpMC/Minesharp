using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Client;

public sealed class LoginStartPacket : LoginPacket
{
    /// <summary>
    ///     Player's Username.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///     Whether or not the next fields should be sent.
    /// </summary>
    public bool HasSignature { get; init; }

    /// <summary>
    ///     When the key data will expire.
    ///     Optional. Only sent if Has Sig Data is true.
    /// </summary>
    public long Timestamp { get; init; }

    /// <summary>
    ///     The encoded bytes of the public key the client received from Mojang.
    ///     Optional. Only sent if Has Sig Data is true.
    /// </summary>
    public byte[] PublicKey { get; init; }

    /// <summary>
    ///     The bytes of the public key signature the client received from Mojang.
    ///     Optional. Only sent if Has Sig Data is true.
    /// </summary>
    public byte[] Signature { get; init; }
}

public sealed class LoginStartPacketCodec : LoginPacketDecoder<LoginStartPacket>
{
    public override int PacketId => 0x00;

    protected override LoginStartPacket Decode(IByteBuffer buffer)
    {
        var username = buffer.ReadString();
        var hasSignature = buffer.ReadBoolean();
        long timestamp = 0;
        var publicKey = Array.Empty<byte>();
        var signature = Array.Empty<byte>();

        if (hasSignature)
        {
            timestamp = buffer.ReadLong();
            publicKey = buffer.ReadByteArray();
            signature = buffer.ReadByteArray();
        }

        return new LoginStartPacket
        {
            Username = username,
            HasSignature = hasSignature,
            Timestamp = timestamp,
            PublicKey = publicKey,
            Signature = signature
        };
    }
}