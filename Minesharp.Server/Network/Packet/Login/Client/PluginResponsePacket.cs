using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Client;

public sealed class PluginResponsePacket : LoginPacket
{
    /// <summary>
    ///     Should match ID from server
    /// </summary>
    public int MessageId { get; init; }

    /// <summary>
    ///     true if the client understood the request, false otherwise.
    ///     When false, no payload follows.
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    ///     Any data, depending on the channel.
    ///     The length of this array must be inferred from the packet length.
    /// </summary>
    public byte[] Data { get; init; } = Array.Empty<byte>();
}

public sealed class PluginResponsePacketCodec : LoginPacketCodec<PluginResponsePacket>
{
    public override int PacketId => 0x02;

    protected override PluginResponsePacket Decode(IByteBuffer buffer)
    {
        var messageId = buffer.ReadVarInt();
        var successful = buffer.ReadBoolean();
        var data = Array.Empty<byte>();

        if (successful)
        {
            data = buffer.ReadBytes(buffer.ReadableBytes).Array;
        }

        return new PluginResponsePacket
        {
            MessageId = messageId,
            IsSuccessful = successful,
            Data = data
        };
    }

    protected override void Encode(PluginResponsePacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.MessageId);
        buffer.WriteBoolean(packet.IsSuccessful);

        if (packet.IsSuccessful)
        {
            buffer.WriteBytes(packet.Data);
        }
    }
}