using Minesharp.Blocks;
using Minesharp.Entities;

namespace Minesharp.Events.Block;

/// <summary>
///     Event called when a block is break by a player
/// </summary>
public class BlockBreakEvent : IEvent
{
    public BlockBreakEvent(IBlock block, IPlayer player)
    {
        Block = block;
        Player = player;
    }

    /// <summary>
    ///     Block broken
    /// </summary>
    public IBlock Block { get; }

    /// <summary>
    ///     Player who break the block
    /// </summary>
    public IPlayer Player { get; }

    /// <summary>
    ///     Define if this event should be cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}