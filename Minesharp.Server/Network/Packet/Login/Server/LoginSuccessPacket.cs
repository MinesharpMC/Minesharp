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

public sealed class LoginSuccessPacketCodec : LoginPacketCodec<LoginSuccessPacket>
{
    public override int PacketId => 0x02;

    protected override LoginSuccessPacket Decode(IByteBuffer buffer)
    {
        var id = buffer.ReadGuid();
        var username = buffer.ReadString();

        var length = buffer.ReadVarInt();
        var properties = new LoginProperty[length];
        for (var i = 0; i < length; i++)
        {
            var propertyName = buffer.ReadString();
            var propertyValue = buffer.ReadString();
            var propertyIsSigned = buffer.ReadBoolean();
            var propertySignature = string.Empty;

            if (propertyIsSigned)
            {
                propertySignature = buffer.ReadString();
            }

            properties[i] = new LoginProperty
            {
                Name = propertyName,
                Value = propertyValue,
                IsSigned = propertyIsSigned,
                Signature = propertySignature
            };
        }

        return new LoginSuccessPacket
        {
            Id = id,
            Username = username,
            Properties = properties
        };
    }

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