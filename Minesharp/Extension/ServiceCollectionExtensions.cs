
using Minesharp.Configuration;
using Minesharp.Game;
using Minesharp.Game.Entities;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Network.Packet;
using Minesharp.Network.Processor;

namespace Minesharp.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddServer(this IServiceCollection services)
    {
        services.AddSingleton<Server>();
        services.AddSingleton<PlayerManager>();
        services.AddSingleton<WorldManager>();

        services.AddHostedService<ServerService>();
        services.AddHostedService<WorldService>();
    }

    public static void AddServerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        
    }

    public static void AddNetworkConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Network");
        if (!section.Exists())
        {
            throw new InvalidOperationException();
        }

        services.AddSingleton(section.Get<NetworkConfiguration>());
    }
    
    public static void AddNetworkServer(this IServiceCollection services)
    {
        services.AddSingleton<NetworkServer>();
        services.AddHostedService<ServerService>();
    }

    public static void AddPacketFactory(this IServiceCollection services)
    {
        var creators = typeof(PacketCreator).Assembly.GetTypes()
            .Where(x => typeof(PacketCreator).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .ToList();
        
        var buffers = typeof(BufferCreator).Assembly.GetTypes()
            .Where(x => typeof(BufferCreator).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract);

        foreach (var type in creators)
        {
            services.AddSingleton(typeof(PacketCreator), type);
        }
        
        foreach (var type in buffers)
        {
            services.AddSingleton(typeof(BufferCreator), type);
        }
        
        services.AddSingleton<PacketFactory>();
    }

    public static void AddPacketProcessor(this IServiceCollection services)
    {
        var types = typeof(PacketProcessor).Assembly.GetTypes()
            .Where(x => typeof(PacketProcessor).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract);

        foreach (var type in types)
        {
            services.AddSingleton(typeof(PacketProcessor), type);
        }

        services.AddSingleton<PacketProcessorManager>();
    }
}