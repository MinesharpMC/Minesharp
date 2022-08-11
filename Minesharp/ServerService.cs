using Minesharp.Game;
using Minesharp.Network.Packet.Server.Play;

namespace Minesharp.Network;

public class ServerService : IHostedService
{
    private readonly Server server;
    private readonly NetworkServer networkServer;
    private readonly ILogger<ServerService> logger;
    private readonly CancellationTokenSource cts = new();

    private Task task;

    public ServerService(Server server, ILogger<ServerService> logger, NetworkServer networkServer)
    {
        this.server = server;
        this.logger = logger;
        this.networkServer = networkServer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await networkServer.StartAsync();
        logger.LogInformation("Server started and running on {ip}", networkServer.Ip);

        task = ExecuteAsync(cts.Token);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        cts.Cancel();
        if (task is not null)
        {
            await task;
        }
        
        await networkServer.StopAsync();
        logger.LogInformation("Server running on {ip} successfully stopped", networkServer.Ip);
    }

    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            server.Tick();
            await Task.Delay(100);
        }
    }
}