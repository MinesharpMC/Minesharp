using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class PluginRequestPacket : LoginPacket
{
    /// <summary>
    ///     Generated by the server - should be unique to the connection
    /// </summary>
    public int MessageId { get; init; }

    /// <summary>
    ///     Name of the plugin channel used to send the data.
    /// </summary>
    public Guid Channel { get; init; }

    /// <summary>
    ///     Any data, depending on the channel.
    ///     The length of this array must be inferred from the packet length.
    /// </summary>
    public byte[] Data { get; init; }
}

public sealed class PluginRequestPacketCodec : LoginPacketCodec<PluginRequestPacket>
{
    public override int PacketId => 0x04;

    protected override PluginRequestPacket Decode(IByteBuffer buffer)
    {
        var messageId = buffer.ReadVarInt();
        var channel = buffer.ReadGuid();
        var data = buffer.ReadBytes(buffer.ReadableBytes);

        return new PluginRequestPacket
        {
            MessageId = messageId,
            Channel = channel,
            Data = data.Array
        };
    }

    protected override void Encode(PluginRequestPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.MessageId);
        buffer.WriteGuid(packet.Channel);
        buffer.WriteBytes(packet.Data);
    }
}