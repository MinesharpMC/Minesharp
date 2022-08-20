using Minesharp.Blocks;
using Minesharp.Worlds;

namespace Minesharp.Entities;

/// <summary>
///     Represent any kind of entity
/// </summary>
public interface IEntity
{
    /// <summary>
    ///     Id of this entity
    /// </summary>
    int Id { get; }

    /// <summary>
    ///     Unique identifier for this entity
    /// </summary>
    Guid UniqueId { get; }

    /// <summary>
    ///     Position of this entity
    /// </summary>
    Position Position { get; }

    /// <summary>
    ///     Rotation of this entity
    /// </summary>
    Rotation Rotation { get; }

    /// <summary>
    ///     Get the world of this entity
    /// </summary>
    /// <returns>The world where this entity is currently</returns>
    IWorld GetWorld();
    
    double Height { get; }
    double Width { get; }
    
    BoundingBox BoundingBox { get; }
}