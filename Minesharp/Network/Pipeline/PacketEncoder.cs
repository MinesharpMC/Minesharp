using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Extension;
using Minesharp.Network.Packet;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Packet.Server;
using Serilog;

namespace Minesharp.Network.Pipeline;

public class PacketEncoder : MessageToByteEncoder<ServerPacket>
{
    private readonly NetworkClient client;
    private readonly PacketFactory factory;

    public PacketEncoder(NetworkClient client, PacketFactory factory)
    {
        this.client = client;
        this.factory = factory;
    }

    protected override void Encode(IChannelHandlerContext context, ServerPacket message, IByteBuffer output)
    {
        var buffer = factory.CreateBuffer(client.Protocol, message);
        if (buffer is null)
        {
            return;
        }
        
        output.WriteVarInt(buffer.ReadableBytes);
        output.WriteBytes(buffer);
    }
}