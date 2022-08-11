using Minesharp.Extension;

namespace Minesharp;

public class Startup
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
        
        services.AddPacketFactory();
        services.AddPacketProcessor();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(x =>
        {
            x.MapControllers();
        });
    }
}