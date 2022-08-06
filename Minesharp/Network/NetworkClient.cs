using DotNetty.Transport.Channels;
using Minesharp.Game.Entities;
using Minesharp.Network.Common;
using Minesharp.Network.Packet.Server;

namespace Minesharp.Network;

public class NetworkClient
{
    public Player Player { get; set; }
    
    public NetworkProtocol Protocol { get; set; }

    private readonly IChannel channel;

    public NetworkClient(IChannel channel)
    {
        this.channel = channel;
    }

    public void SendPacket(ServerPacket packet)
    {
        channel.WriteAndFlushAsync(packet);
    }
}