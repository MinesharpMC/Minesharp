using Minesharp.Server.Game;
using Minesharp.Server.Game.Chunks.Generator;
using Minesharp.Server.Game.Plugins;
using Minesharp.Server.Game.Worlds;
using Minesharp.Server.Network;

namespace Minesharp.Server;

public class ServerService : BackgroundService
{
    private readonly ILogger<ServerService> logger;
    private readonly NetworkServer networkServer;
    private readonly GameServer server;

    public ServerService(GameServer server, ILogger<ServerService> logger, NetworkServer networkServer)
    {
        this.server = server;
        this.logger = logger;
        this.networkServer = networkServer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Loading block registry");
        server.BlockRegistry.Load();

        logger.LogInformation("Creating world");
        var world = server.CreateWorld(new WorldCreator
        {
            Name = "Debug World",
            ChunkGenerator = new SuperflatGenerator(server),
            Difficulty = Difficulty.Normal,
            GameMode = GameMode.Survival,
            SpawnPosition = new Position(0, -59, 0)
        });

        if (world is null)
        {
            logger.LogError("Failed to create world");
            return;
        }
        
        logger.LogInformation("Starting plugins");
        await server.PluginManager.StartAll();

        logger.LogInformation("Starting server");
        await networkServer.StartAsync();

        logger.LogInformation("Server is now running");

        while (!stoppingToken.IsCancellationRequested)
        {
            server.Tick();
        }

        logger.LogInformation("Stopping server");
        await networkServer.StopAsync();
        
        logger.LogInformation("Stopping plugins");
        await server.PluginManager.StopAll();
        
        logger.LogInformation("Server is now stopped");
    }
}