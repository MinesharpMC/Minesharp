using Minesharp.Game.Worlds;
using Minesharp.Network;

namespace Minesharp.Game.Entities;

public class Player
{
    public World World { get; set; }
    public Position Position { get; set; }
    public NetworkClient Client { get; init; }
}