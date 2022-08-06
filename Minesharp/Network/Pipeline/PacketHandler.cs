using DotNetty.Transport.Channels;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Processor;

namespace Minesharp.Network.Pipeline;

public class PacketHandler : SimpleChannelInboundHandler<ClientPacket>
{
    private readonly NetworkClient client;
    private readonly PacketProcessorManager processorManager;

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
            return;
        }
        
        processor.Process(client, msg);
    }
}