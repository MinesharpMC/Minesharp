using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Minesharp.Packet.Extension;

namespace Minesharp.Network.Pipeline;

public class FrameDecoder : ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        input.MarkReaderIndex();
        if (!input.ReadableVarInt())
        {
            return;
        }

        var length = input.ReadVarInt();
        if (input.ReadableBytes < length)
        {
            input.ResetReaderIndex();
            return;
        }

        var buffer = context.Allocator.Buffer(length);
        input.ReadBytes(buffer, length);
        output.Add(buffer);
    }
}