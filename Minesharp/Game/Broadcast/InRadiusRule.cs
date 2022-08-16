using Minesharp.Common;
using Minesharp.Game.Entities;

namespace Minesharp.Game.Broadcast;

public class InRadiusRule : IBroadcastRule
{
    private readonly int radius;
    private readonly Position position;

    public InRadiusRule(Position position, int radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public bool IsAllowed(Player player)
    {
        var radiusSquared = radius * radius;
        var squared = player.Position.DistanceSquared(position);
        return squared <= radiusSquared;
    }
}