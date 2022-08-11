using Minesharp.Network.Packet.Server.Play;

namespace Minesharp.Network;

public class NetworkService : IHostedService
{
    private readonly NetworkServer server;
    private readonly ILogger<NetworkService> logger;
    private readonly NetworkSessionManager sessionManager;
    private readonly CancellationTokenSource cts = new();

    private Task task;

    public NetworkService(NetworkServer server, ILogger<NetworkService> logger, NetworkSessionManager sessionManager)
    {
        this.server = server;
        this.logger = logger;
        this.sessionManager = sessionManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await server.StartAsync();

        task = ExecuteAsync(cts.Token);
        
        logger.LogInformation("Server started and running on {ip}", server.Ip);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        cts.Cancel();
        await task;
        await server.StopAsync();
        logger.LogInformation("Server running on {ip} successfully stopped", server.Ip);
    }

    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var sessions = sessionManager.GetSessions();
            foreach (var session in sessions)
            {
                session.Tick();
            }

            await Task.Delay(50);
        }
    }
}