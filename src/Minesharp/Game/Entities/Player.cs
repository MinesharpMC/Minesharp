using Minesharp.Network;

namespace Minesharp.Game.Entity;

public class Player
{
    private readonly NetworkClient client;

    public Player(NetworkClient client)
    {
        this.client = client;
    }
}