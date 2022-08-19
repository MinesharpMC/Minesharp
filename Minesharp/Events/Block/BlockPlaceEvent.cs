using Minesharp.Blocks;
using Minesharp.Entities;

namespace Minesharp.Events.Block;

public class BlockPlaceEvent : IEvent
{
    public BlockPlaceEvent(IBlock block, IPlayer player)
    {
        Block = block;
        Player = player;
    }

    public IBlock Block { get; }
    public IPlayer Player { get; }
    
    public bool IsCancelled { get; set; }
}