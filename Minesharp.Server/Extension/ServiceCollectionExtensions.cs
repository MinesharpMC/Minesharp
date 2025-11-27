using Minesharp.Server.Blocks;
using Minesharp.Server.Common.Configuration;
using Minesharp.Server.Network;
using Minesharp.Server.Network.Packet;
using Minesharp.Server.Network.Processor;

namespace Minesharp.Server.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddServer(this IServiceCollection services)
    {
        services.AddSingleton<GameServer>();
        services.AddHostedService<ServerService>();
    }

    public static void AddRegistry(this IServiceCollection services)
    {
        services.AddSingleton<BlockRegistry>();
    }

    public static void AddNetworkServer(this IServiceCollection services)
    {
        services.AddSingleton<NetworkServer>();
        services.AddSingleton<SessionManager>();
    }

    public static void AddNetworkConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration<NetworkConfiguration>(configuration, "network");
    }

    public static void AddServerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration<ServerConfiguration>(configuration, "server");
    }

    private static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        if (!section.Exists())
        {
            throw new InvalidOperationException();
        }

        var mapped = section.Get<T>();
        services.AddSingleton<T>(mapped);
    }

    public static void AddPacketProcessor(this IServiceCollection services)
    {
        var types = typeof(IPacketProcessor).Assembly.GetTypes()
            .Where(x => typeof(IPacketProcessor).IsAssignableFrom(x))
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsInterface);

        foreach (var type in types)
        {
            services.AddTransient(typeof(IPacketProcessor), type);
        }

        services.AddSingleton<PacketProcessorManager>();
    }

    public static void AddPacketFactory(this IServiceCollection services)
    {
        var decoders = typeof(IPacketDecoder).Assembly.GetTypes()
            .Where(x => typeof(IPacketDecoder).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .ToList();
        
        var encoders = typeof(IPacketEncoder).Assembly.GetTypes()
            .Where(x => typeof(IPacketEncoder).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .ToList();

        foreach (var type in decoders)
        {
            services.AddSingleton(typeof(IPacketDecoder), type);
        }

        foreach (var type in encoders)
        {
            services.AddSingleton(typeof(IPacketEncoder), type);
        }

        services.AddSingleton<PacketFactory>();
    }
}