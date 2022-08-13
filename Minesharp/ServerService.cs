using Minesharp.Game;
using Minesharp.Game.Chunks.Generator;
using Minesharp.Game.Worlds;
using Minesharp.Network;

namespace Minesharp;

public class ServerService : BackgroundService
{
    private readonly ILogger<ServerService> logger;
    private readonly NetworkServer networkServer;
    private readonly Server server;
    private readonly PeriodicTimer timer = new(TimeSpan.FromMilliseconds(100));

    public ServerService(Server server, ILogger<ServerService> logger, NetworkServer networkServer)
    {
        this.server = server;
        this.logger = logger;
        this.networkServer = networkServer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Creating world");
        var world = server.CreateWorld(new WorldCreator
        {
            Name = "Debug World",
            Border = new WorldBorder(),
            ChunkGenerator = new SuperflatGenerator()
        });

        if (world is null)
        {
            logger.LogError("Failed to create world");
            return;
        }

        logger.LogInformation("Starting server");
        await networkServer.StartAsync();

        logger.LogInformation("Server is now running");

        while (!stoppingToken.IsCancellationRequested)
        {
            server.Tick();
            await timer.WaitForNextTickAsync();
        }

        logger.LogInformation("Stopping server");
        await networkServer.StopAsync();
        logger.LogInformation("Server is now stopped");
    }
}