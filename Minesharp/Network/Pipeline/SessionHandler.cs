using DotNetty.Transport.Channels;
using Serilog;

namespace Minesharp.Network.Pipeline;

public class SessionHandler : ChannelHandlerAdapter
{
    private readonly NetworkSession session;

    public SessionHandler(NetworkSession session)
    {
        this.session = session;
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Log.Error(exception, "Something happened with session");
        base.ExceptionCaught(context, exception);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        var player = session.Player;
        if (session.Player is not null)
        {
            player.World.Remove(session.Player);
            Log.Information("{name} disconnected", session.Player.Username);
        }
        base.ChannelInactive(context);
    }
}