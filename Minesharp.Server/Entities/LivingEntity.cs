using Minesharp.Server.Worlds;

namespace Minesharp.Server.Entities;

public abstract class LivingEntity : Entity
{
    public double Health { get; set; }
    public double MaximumHealth { get; set; }

    protected LivingEntity(World world, Position position) : base(world, position)
    {
        
    }
}