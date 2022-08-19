using Minesharp.Server.Configuration;
using Minesharp.Server.Game;
using Minesharp.Server.Game.Blocks;
using Minesharp.Server.Game.Managers;
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
        var codecs = typeof(IPacketCodec).Assembly.GetTypes()
            .Where(x => typeof(IPacketCodec).IsAssignableFrom(x))
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .ToList();

        foreach (var type in codecs)
        {
            services.AddSingleton(typeof(IPacketCodec), type);
        }

        services.AddSingleton<PacketFactory>();
    }
}