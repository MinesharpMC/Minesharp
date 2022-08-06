using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;

namespace Minesharp.Server;

public class NetworkServer
{
    private ServerBootstrap bootstrap;
    private readonly MultithreadEventLoopGroup bossGroup;
    private readonly MultithreadEventLoopGroup workerGroup;

    public NetworkServer()
    {
        
    }
}