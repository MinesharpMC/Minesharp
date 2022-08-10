using Minesharp.Game.Chunks.Generator;

namespace Minesharp.Game.Worlds;

public class WorldService : IHostedService
{
    private readonly WorldManager worldManager;
    private readonly ILogger<WorldService> logger;

    public WorldService(WorldManager worldManager, ILogger<WorldService> logger)
    {
        this.worldManager = worldManager;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var creator = new WorldCreator("debug")
        {
            ChunkGenerator = new SuperflatGenerator()
        };

        var world = worldManager.CreateWorld(creator);
        if (world is null)
        {
            throw new InvalidOperationException();
        }
        
        logger.LogInformation("Successfully created or loaded world {name}",  world.Name);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}