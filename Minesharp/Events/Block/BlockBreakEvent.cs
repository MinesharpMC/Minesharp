using Minesharp.Blocks;

namespace Minesharp.Events.Block;

public class BlockBreakEvent : IEvent
{
    public IBlock Block { get; }
    public bool IsCancelled { get; set; }
    
    public BlockBreakEvent(IBlock block)
    {
        Block = block;
    }
}