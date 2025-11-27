using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class LoginSuccessPacket : LoginPacket
{
    /// <summary>
    ///     Session unique id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Player username
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///     Properties (can be empty)
    /// </summary>
    public LoginProperty[] Properties { get; init; } = Array.Empty<LoginProperty>();
}

public sealed class LoginProperty
{
    /// <summary>
    ///     Name of this property
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Value of this property
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    ///     Defined if Signature value is set
    /// </summary>
    public bool IsSigned { get; init; }

    /// <summary>
    ///     Signature value (only set if IsSigned is true)
    /// </summary>
    public string Signature { get; init; }
}

public sealed class LoginSuccessPacketCodec : LoginPacketEncoder<LoginSuccessPacket>
{
    public override int PacketId => 0x02;

    protected override void Encode(LoginSuccessPacket packet, IByteBuffer buffer)
    {
        buffer.WriteGuid(packet.Id);
        buffer.WriteString(packet.Username);

        buffer.WriteVarInt(packet.Properties.Length);
        foreach (var property in packet.Properties)
        {
            buffer.WriteString(property.Name);
            buffer.WriteString(property.Value);
            buffer.WriteBoolean(property.IsSigned);

            if (property.IsSigned)
            {
                buffer.WriteString(property.Signature);
            }
        }
    }
}