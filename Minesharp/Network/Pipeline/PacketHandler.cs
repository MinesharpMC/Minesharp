using DotNetty.Transport.Channels;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Processor;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Minesharp.Network.Pipeline;

public class PacketHandler : SimpleChannelInboundHandler<ClientPacket>
{
    private readonly NetworkClient client;
    private readonly PacketProcessorManager processorManager;
    private readonly ILogger logger;

    public PacketHandler(NetworkClient client, PacketProcessorManager processorManager)
    {
        this.client = client;
        this.processorManager = processorManager;
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, ClientPacket msg)
    {
        var processor = processorManager.GetProcessorForPacket(msg);
        if (processor is null)
        {
            Log.Warning("No packet processor found for packet {packet}", msg.GetType().Name);
            return;
        }
        
        processor.Process(client, msg);
    }
}