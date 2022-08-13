using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Minesharp.Network.Pipeline;
using Minesharp.Network.Processor;
using Minesharp.Packet;

namespace Minesharp.Network;

public sealed class NetworkServer
{
    private readonly ServerBootstrap bootstrap;
    private readonly MultithreadEventLoopGroup bossGroup, workerGroup;

    private IChannel channel;

    public NetworkServer(PacketFactory packetFactory, PacketProcessorManager processorManager)
    {
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
                pipeline.AddLast(new SessionHandler(session));
            }));
    }

    public async Task StartAsync()
    {
        channel = await bootstrap.BindAsync(25565);
    }

    public async Task StopAsync()
    {
        await channel.CloseAsync();
        await bossGroup.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
        await workerGroup.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero);
    }
}