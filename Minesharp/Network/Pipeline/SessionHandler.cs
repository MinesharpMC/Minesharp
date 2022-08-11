using DotNetty.Transport.Channels;
using Minesharp.Game;
using Minesharp.Game.Entities;

namespace Minesharp.Network.Pipeline;

public class SessionHandler : ChannelHandlerAdapter
{
    private readonly Server server;
    private readonly NetworkSession session;

    public SessionHandler(NetworkSession session, Server server)
    {
        this.session = session;
        this.server = server;
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        if (session.Player is not null)
        {
            server.RemovePlayer(session.Player);
        }
        
        base.ChannelInactive(context);
    }
}