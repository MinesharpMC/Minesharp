using DotNetty.Transport.Channels;
using Minesharp.Server.Network.Packet;
using Minesharp.Server.Network.Packet.Game;
using Minesharp.Server.Network.Processor;
using Serilog;

namespace Minesharp.Server.Network.Pipeline;

public class PacketHandler : SimpleChannelInboundHandler<IPacket>
{
    private readonly PacketProcessorManager processorManager;
    private readonly NetworkSession session;

    public PacketHandler(NetworkSession session, PacketProcessorManager processorManager)
    {
        this.session = session;
        this.processorManager = processorManager;
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, IPacket msg)
    {
        if (msg is GamePacket) // Game packets are handled in thread loop
        {
            session.Enqueue(msg);
            return;
        }

        var processor = processorManager.GetProcessor(msg.GetType());
        if (processor is null)
        {
            Log.Warning("Can't found packet processor for packet {packet}", msg.GetType().Name);
            return;
        }

        processor.Process(session, msg);
    }
}