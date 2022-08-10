using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Extension;
using Minesharp.Network.Packet;

namespace Minesharp.Network.Pipeline;

public class PacketDecoder : ByteToMessageDecoder
{
    private readonly NetworkClient client;
    private readonly PacketFactory factory;

    public PacketDecoder(NetworkClient client, PacketFactory factory)
    {
        this.client = client;
        this.factory = factory;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var packet = factory.CreatePacket(client.Protocol, input);
        if (packet is null)
        {
            input.Clear();
            return;
        }
        
        output.Add(packet);
    }
}