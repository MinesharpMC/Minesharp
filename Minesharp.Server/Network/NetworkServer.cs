using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Minesharp.Server.Configuration;
using Minesharp.Server.Game.Managers;
using Minesharp.Server.Network.Packet;
using Minesharp.Server.Network.Pipeline;
using Minesharp.Server.Network.Processor;

namespace Minesharp.Server.Network;

public sealed class NetworkServer
{
    private readonly ServerBootstrap bootstrap;
    private readonly MultithreadEventLoopGroup bossGroup, workerGroup;
    private readonly NetworkConfiguration configuration;

    private IChannel channel;

    public NetworkServer(PacketFactory packetFactory, PacketProcessorManager processorManager, NetworkConfiguration configuration, SessionManager sessionManager)
    {
        this.configuration = configuration;
        bossGroup = new MultithreadEventLoopGroup(1);
        workerGroup = new MultithreadEventLoopGroup();

        bootstrap = new ServerBootstrap()
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .ChildHandler(new ActionChannelInitializer<IChannel>(x =>
            {
                var pipeline = x.Pipeline;

                var session = new NetworkSession(x, processorManager);

                pipeline.AddLast(new FrameDecoder());
                pipeline.AddLast(new PacketDecoder(session, packetFactory));
                pipeline.AddLast(new PacketEncoder(session, packetFactory));
                pipeline.AddLast(new PacketHandler(session, processorManager));
                pipeline.AddLast(new SessionHandler(session, sessionManager));
            }));
    }

    public async Task StartAsync()
    {
        channel = await bootstrap.BindAsync(configuration.Port);
    }

    public async Task StopAsync()
    {
        await channel.CloseAsync();
        await bossGroup.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
        await workerGroup.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
    }
}