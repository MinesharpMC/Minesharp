namespace Minesharp.Entities;

/// <summary>
///     Represent a living entity (with health)
/// </summary>
public interface ILivingEntity : IEntity
{
    /// <summary>
    ///     Health of this entity
    /// </summary>
    public double Health { get; }

    /// <summary>
    ///     Maximum health of this entity
    /// </summary>
    public double MaximumHealth { get; }
}