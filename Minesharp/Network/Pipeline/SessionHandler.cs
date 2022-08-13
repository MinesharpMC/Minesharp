using DotNetty.Transport.Channels;

namespace Minesharp.Network.Pipeline;

public class SessionHandler : ChannelHandlerAdapter
{
    private readonly NetworkSession session;

    public SessionHandler(NetworkSession session)
    {
        this.session = session;
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var player = session.Player;
        if (session.Player is not null)
        {
            player.World.Remove(session.Player);
        }

        base.ChannelInactive(context);
    }
}