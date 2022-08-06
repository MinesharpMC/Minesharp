using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Extension;

namespace Minesharp.Network.Pipeline;

public class MessageDecoder : ByteToMessageDecoder
{
    private readonly 
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var packetId = input.ReadVarInt();

    }
}