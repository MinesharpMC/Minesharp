using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class CompressionPacket : LoginPacket
{
    /// <summary>
    ///     Maximum size of a packet before it is compressed.
    /// </summary>
    public int Threshold { get; init; }
}

public sealed class CompressionPacketCodec : LoginPacketCodec<CompressionPacket>
{
    public override int PacketId => 0x03;

    protected override CompressionPacket Decode(IByteBuffer buffer)
    {
        var threshold = buffer.ReadVarInt();

        return new CompressionPacket
        {
            Threshold = threshold
        };
    }

    protected override void Encode(CompressionPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.Threshold);
    }
}