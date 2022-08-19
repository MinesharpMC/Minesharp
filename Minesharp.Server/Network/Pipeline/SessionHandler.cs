using DotNetty.Transport.Channels;
using Minesharp.Events.Player;
using Minesharp.Server.Extension;
using Serilog;

namespace Minesharp.Server.Network.Pipeline;

public class SessionHandler : ChannelHandlerAdapter
{
    private readonly NetworkSession session;
    private readonly SessionManager sessionManager;

    public SessionHandler(NetworkSession session, SessionManager sessionManager)
    {
        this.session = session;
        this.sessionManager = sessionManager;
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Log.Error(exception, "Something happened with session");
        base.ExceptionCaught(context, exception);
    }

    public override void ChannelActive(IChannelHandlerContext context)
    {
        sessionManager.Add(session);
        base.ChannelActive(context);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var player = session.Player;
        if (player is not null)
        {
            player.Server.CallEvent(new PlayerDisconnectEvent(player));

            player.World.RemovePlayer(player);
            player.Server.RemovePlayer(player);
            player.Server.BroadcastPlayerListRemove(player);

            Log.Information("{name} disconnected", session.Player.Username);
        }

        sessionManager.Remove(session);

        base.ChannelInactive(context);
    }
}