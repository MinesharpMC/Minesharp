using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Server.Network.Packet;
using Serilog;

namespace Minesharp.Server.Network.Pipeline;

public class PacketDecoder : ByteToMessageDecoder
{
    private readonly PacketFactory factory;
    private readonly NetworkSession session;

    public PacketDecoder(NetworkSession session, PacketFactory factory)
    {
        this.session = session;
        this.factory = factory;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var packet = factory.Decode(session.Protocol, input);
        if (packet is null)
        {
            input.Clear();
            return;
        }

        output.Add(packet);

        Log.Debug("Receiving packet: {message}", packet.GetType().Name);
    }
}