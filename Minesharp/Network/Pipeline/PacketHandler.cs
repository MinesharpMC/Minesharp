using DotNetty.Transport.Channels;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Processor;
using Serilog;

namespace Minesharp.Network.Pipeline;

public class PacketHandler : SimpleChannelInboundHandler<ClientPacket>
{
    private readonly NetworkSession session;
    private readonly PacketProcessorManager processorManager;

    public PacketHandler(NetworkSession session, PacketProcessorManager processorManager)
    {
        this.session = session;
        this.processorManager = processorManager;
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, ClientPacket msg)
    {
        if (session.Protocol == NetworkProtocol.Play)
        {
            session.ReceivePacket(msg);
            return;
        }
        
        var processor = processorManager.GetProcessorForPacket(msg);
        if (processor is null)
        {
            Log.Warning("Can't found packet processor for packet {packet}", msg.GetType().Name);
            return;
        }
            
        processor.Process(session, msg);
    }
}