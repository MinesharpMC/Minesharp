using DotNetty.Transport.Channels;
using Minesharp.Game.Entities;
using Minesharp.Network.Common;
using Minesharp.Network.Packet.Server;
using Serilog;

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
        channel.WriteAndFlushAsync(packet).ContinueWith(x =>
        {
            Log.Error(x.Exception!.InnerException!, "Failed to send packet");
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    public void Disconnect()
    {
        channel.CloseAsync().ContinueWith(x =>
        {
            Log.Error(x.Exception!.InnerException!, "Failed to disconnect");
        }, TaskContinuationOptions.OnlyOnFaulted);
    }
}