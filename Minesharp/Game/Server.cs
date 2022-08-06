using Minesharp.Configuration;

namespace Minesharp.Game;

public sealed class Server
{
    public string Version => "1.19";
    public int Protocol => 759;

    public int MaxPlayers => configuration.MaxPlayers;
    public string Description => configuration.Description;

    private readonly ServerConfiguration configuration;
    
    public Server(ServerConfiguration configuration)
    {
        this.configuration = configuration;
    }
}