using DotNetty.Transport.Channels;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Processor;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Minesharp.Network.Pipeline;

public class SessionHandler : SimpleChannelInboundHandler<ClientPacket>
{
    private readonly NetworkSession session;
    private readonly NetworkSessionManager sessionManager;
    private readonly PacketProcessorManager processorManager;

    public SessionHandler(NetworkSession session, NetworkSessionManager sessionManager, PacketProcessorManager processorManager)
    {
        this.session = session;
        this.sessionManager = sessionManager;
        this.processorManager = processorManager;
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, ClientPacket msg)
    {
        var processor = processorManager.GetProcessorForPacket(msg);
        if (processor is null)
        {
            Log.Warning("Can't found packet processor for packet {packet}", msg.GetType().Name);
            return;
        }
            
        processor.Process(session, msg);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        sessionManager.RemoveSession(session);
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        sessionManager.AddSession(session);
    }
}