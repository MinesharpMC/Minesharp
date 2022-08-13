using Minesharp.Game;
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

    public static void AddNetworkServer(this IServiceCollection services)
    {
        services.AddSingleton<NetworkServer>();
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