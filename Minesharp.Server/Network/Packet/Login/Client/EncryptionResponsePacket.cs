using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Client;

public sealed class EncryptionResponsePacket : LoginPacket
{
    /// <summary>
    ///     Shared Secret value, encrypted with the server's public key.
    /// </summary>
    public byte[] SharedSecret { get; init; }

    /// <summary>
    ///     Whether or not the Verify Token should be sent.
    ///     If not, then the salt and signature will be sent.
    /// </summary>
    public bool HasToken { get; init; }

    /// <summary>
    ///     Verify Token value, encrypted with the same public key as the shared secret.
    ///     Optional and only sent if Has Verify Token is true.
    /// </summary>
    public byte[] Token { get; init; }

    /// <summary>
    ///     Cryptography, used for validating the message signature.
    ///     Optional and only sent if Has Verify Token is false.
    /// </summary>
    public long Salt { get; init; }

    /// <summary>
    ///     The bytes of the public key signature the client received from Mojang.
    ///     Optional and only sent if Has Verify Token is false.
    /// </summary>
    public byte[] Signature { get; init; }
}

public sealed class EncryptionResponsePacketCodec : LoginPacketCodec<EncryptionResponsePacket>
{
    public override int PacketId => 0x01;

    protected override EncryptionResponsePacket Decode(IByteBuffer buffer)
    {
        var sharedSecret = buffer.ReadByteArray();
        var hasToken = buffer.ReadBoolean();
        var token = Array.Empty<byte>();
        long salt = 0;
        var signature = Array.Empty<byte>();

        if (hasToken)
        {
            token = buffer.ReadByteArray();
        }
        else
        {
            salt = buffer.ReadLong();
            signature = buffer.ReadByteArray();
        }

        return new EncryptionResponsePacket
        {
            SharedSecret = sharedSecret,
            HasToken = hasToken,
            Token = token,
            Salt = salt,
            Signature = signature
        };
    }

    protected override void Encode(EncryptionResponsePacket packet, IByteBuffer buffer)
    {
        buffer.WriteByteArray(packet.SharedSecret);
        buffer.WriteBoolean(packet.HasToken);

        if (packet.HasToken)
        {
            buffer.WriteByteArray(packet.Token);
        }
        else
        {
            buffer.WriteLong(packet.Salt);
            buffer.WriteByteArray(packet.Signature);
        }
    }
}