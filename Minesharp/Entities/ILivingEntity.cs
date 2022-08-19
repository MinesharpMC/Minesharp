namespace Minesharp.Entities;

public interface ILivingEntity : IEntity
{
    public double Health { get; }
    public double MaximumHealth { get; }
}