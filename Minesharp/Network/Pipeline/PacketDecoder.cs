using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Extension;
using Minesharp.Network.Packet;

namespace Minesharp.Network.Pipeline;

public class PacketDecoder : ByteToMessageDecoder
{
    private readonly NetworkSession session;
    private readonly PacketFactory factory;

    public PacketDecoder(NetworkSession session, PacketFactory factory)
    {
        this.session = session;
        this.factory = factory;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var packet = factory.CreatePacket(session.Protocol, input);
        if (packet is null)
        {
            input.Clear();
            return;
        }
        
        output.Add(packet);
    }
}