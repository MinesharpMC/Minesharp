using DotNetty.Buffers;
using Minesharp.Extension;
using Minesharp.Network.Common;

namespace Minesharp.Network.Packet.Server.Status;

public class ResponsePacket : ServerPacket
{
    public string Json { get; init; }

    public override string ToString()
    {
        return $"{nameof(Json)}: {Json}";
    }
}

public class ResponseCreator : BufferCreator<ResponsePacket>
{
    public override int PacketId => 0x0;
    public override NetworkProtocol Protocol => NetworkProtocol.Status;

    protected override void CreateBuffer(ResponsePacket packet, IByteBuffer buffer)
    {
        buffer.WriteString(packet.Json);
    }
}