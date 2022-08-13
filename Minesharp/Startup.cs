using Minesharp.Extension;
using Minesharp.Packet.Extension;

namespace Minesharp;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServer();
        services.AddNetworkServer();
        services.AddPacketFactory();
        services.AddPacketProcessor();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}