namespace Minesharp.Game.Entities;

public abstract class LivingEntity : Entity
{
    public double Health { get; set; }
    public double MaximumHealth { get; set; }
}