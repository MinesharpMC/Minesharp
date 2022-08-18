using Minesharp.Extension;
using Minesharp.Packet.Extension;

namespace Minesharp;

public sealed class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServer();
        services.AddServerConfiguration(configuration);

        services.AddNetworkServer();
        services.AddNetworkConfiguration(configuration);

        services.AddRegistry();

        services.AddPacketFactory();
        services.AddPacketProcessor();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}