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

public sealed class CompressionPacketCodec : LoginPacketEncoder<CompressionPacket>
{
    public override int PacketId => 0x03;

    protected override void Encode(CompressionPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.Threshold);
    }
}