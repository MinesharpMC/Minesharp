using System.Globalization;
using System.Numerics;
using DotNetty.Buffers;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Server.Login;

public class LoginSuccessPacket : ServerPacket
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public List<LoginSuccessProperty> Properties { get; init; } = new List<LoginSuccessProperty>();
}

public class LoginSuccessProperty
{
    public string Name { get; init; }
    public string Value { get; init; }
    public bool IsSigned { get; init; }
    public string Signature { get; init; }
}

public class LoginSuccessCreator : BufferCreator<LoginSuccessPacket>
{
    public override int PacketId => 0x02;
    public override NetworkProtocol Protocol => NetworkProtocol.Login;

    protected override void CreateBuffer(LoginSuccessPacket packet, IByteBuffer buffer)
    {
        buffer.WriteGuid(packet.Id);
        buffer.WriteString(packet.Username);
        buffer.WriteVarInt(packet.Properties.Count);

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