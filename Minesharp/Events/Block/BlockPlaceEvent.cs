using Minesharp.Blocks;
using Minesharp.Entities;

namespace Minesharp.Events.Block;

/// <summary>
/// Event called when a player place a block
/// </summary>
public class BlockPlaceEvent : IEvent
{
    public BlockPlaceEvent(IBlock block, IPlayer player)
    {
        Block = block;
        Player = player;
    }

    /// <summary>
    /// Block placed
    /// </summary>
    public IBlock Block { get; }
    
    /// <summary>
    /// Player who placed the block
    /// </summary>
    public IPlayer Player { get; }
    
    /// <summary>
    /// Define if this event should be cancelled
    /// </summary>
    public bool IsCancelled { get; set; }
}