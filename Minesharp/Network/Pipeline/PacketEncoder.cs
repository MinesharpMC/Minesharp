using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Packet;
using Minesharp.Packet.Extension;
using Minesharp.Packet.Game.Server;
using Serilog;

namespace Minesharp.Network.Pipeline;

public class PacketEncoder : MessageToByteEncoder<IPacket>
{
    private readonly PacketFactory factory;
    private readonly NetworkSession session;

    public PacketEncoder(NetworkSession session, PacketFactory factory)
    {
        this.session = session;
        this.factory = factory;
    }

    protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
    {
        var buffer = Unpooled.Buffer();

        factory.Encode(session.Protocol, buffer, message);

        output.WriteVarInt(buffer.ReadableBytes);
        output.WriteBytes(buffer);
        
        Log.Debug("Sent packet {type} ({packet})", message.GetType().Name, message);
    }
}