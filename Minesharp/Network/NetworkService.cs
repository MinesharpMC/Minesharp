namespace Minesharp.Network;

public class NetworkService : IHostedService
{
    private readonly NetworkServer server;
    private readonly ILogger<NetworkService> logger;

    public NetworkService(NetworkServer server, ILogger<NetworkService> logger)
    {
        this.server = server;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await server.StartAsync();
        logger.LogInformation("Server started and running on {ip}", server.Ip);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await server.StopAsync();
        logger.LogInformation("Server running on {ip} successfully stopped", server.Ip);
    }
}