using System.Net;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Minesharp.Configuration;
using Minesharp.Network.Packet;
using Minesharp.Network.Pipeline;
using Minesharp.Network.Processor;

namespace Minesharp.Network;

public class NetworkServer
{
    private readonly ServerBootstrap bootstrap;
    private readonly MultithreadEventLoopGroup bossGroup;
    private readonly MultithreadEventLoopGroup workerGroup;
    private readonly NetworkConfiguration configuration;
    private readonly ILogger<NetworkServer> logger;
    private IChannel channel;

    public IPEndPoint Ip => configuration.Ip;

    public NetworkServer(NetworkConfiguration configuration, PacketFactory packetFactory, PacketProcessorManager processorManager, ILogger<NetworkServer> logger)
    {
        this.configuration = configuration;
        this.logger = logger;

        this.bossGroup = new MultithreadEventLoopGroup(1);
        this.workerGroup = new MultithreadEventLoopGroup();
        
        this.bootstrap = new ServerBootstrap()
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .ChildHandler(new ActionChannelInitializer<IChannel>(x =>
            {
                var pipeline = x.Pipeline;
                var client = new NetworkClient(x);

                pipeline.AddLast(new FrameDecoder());
                pipeline.AddLast(new PacketDecoder(client, packetFactory));
                pipeline.AddLast(new PacketEncoder(client, packetFactory));
                pipeline.AddLast(new PacketHandler(client, processorManager));
            }));
    }

    public async Task StartAsync()
    {
        channel = await bootstrap.BindAsync(configuration.Ip);
    }

    public async Task StopAsync()
    {
        await channel.CloseAsync();
        await bossGroup.ShutdownGracefullyAsync();
        await workerGroup.ShutdownGracefullyAsync();
    }
}