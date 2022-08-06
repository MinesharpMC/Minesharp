using Minesharp.Network;

namespace Minesharp.Game.Entities;

public class Player
{
    private readonly NetworkClient client;
    
    public Guid Id { get; init; }
    public string Name { get; init; }

    public Player(NetworkClient client)
    {
        this.client = client;
    }
}