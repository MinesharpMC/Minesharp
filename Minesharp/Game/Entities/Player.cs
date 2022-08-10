using Minesharp.Game.Worlds;
using Minesharp.Network;

namespace Minesharp.Game.Entities;

public class Player
{
    public string Name { get; set; }
    public World World { get; set; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
}