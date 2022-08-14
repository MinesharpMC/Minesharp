using Minesharp.Configuration;
using Minesharp.Game;
using Minesharp.Game.Scheduling;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Network.Processor;

namespace Minesharp.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddServer(this IServiceCollection services)
    {
        services.AddSingleton<Server>();
        services.AddHostedService<ServerService>();
    }

    public static void AddServerDependencies(this IServiceCollection services)
    {
        services.AddSingleton<Scheduler>();
        services.AddSingleton<WorldManager>();
    }

    public static void AddNetworkServerDependencies(this IServiceCollection services)
    {
        services.AddSingleton<SessionManager>();
    }

    public static void AddNetworkServer(this IServiceCollection services)
    {
        services.AddSingleton<NetworkServer>();
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
}