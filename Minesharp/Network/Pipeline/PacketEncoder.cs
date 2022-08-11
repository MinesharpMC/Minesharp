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
    private readonly NetworkSession session;
    private readonly PacketFactory factory;

    public PacketEncoder(NetworkSession session, PacketFactory factory)
    {
        this.session = session;
        this.factory = factory;
    }

    protected override void Encode(IChannelHandlerContext context, ServerPacket message, IByteBuffer output)
    {
        var buffer = factory.CreateBuffer(session.Protocol, message);
        if (buffer is null)
        {
            return;
        }
        
        output.WriteVarInt(buffer.ReadableBytes);
        output.WriteBytes(buffer);
    }
}