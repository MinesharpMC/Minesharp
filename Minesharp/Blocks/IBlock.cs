using Minesharp.Worlds;

namespace Minesharp.Blocks;

/// <summary>
///     Represent a block in the world
/// </summary>
public interface IBlock
{
    /// <summary>
    ///     Position of this block
    /// </summary>
    Position Position { get; }

    /// <summary>
    ///     Type of this block
    /// </summary>
    Material Type { get; set; }

    /// <summary>
    ///     Get world of this block
    /// </summary>
    /// <returns>World where this block is currently</returns>
    IWorld GetWorld();
}