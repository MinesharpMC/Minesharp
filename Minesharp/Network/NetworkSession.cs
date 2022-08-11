using System.Collections.Concurrent;
using DotNetty.Transport.Channels;
using Minesharp.Game.Entities;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Packet.Server;
using Minesharp.Network.Packet.Server.Play;
using Minesharp.Network.Processor;
using Serilog;

namespace Minesharp.Network;

public class NetworkSession
{
    public Guid Id { get; } = Guid.NewGuid();
    public NetworkProtocol Protocol { get; set; }
    public Player Player { get; set; }
    
    public long LastKeepAlive { get; set; }
    public DateTime LastKeepAliveSendAt { get; private set; }

    private readonly IChannel channel;

    public NetworkSession(IChannel channel)
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

    public void Tick()
    {
        if (Protocol == NetworkProtocol.Play)
        {
            if (LastKeepAliveSendAt.AddSeconds(10) < DateTime.UtcNow)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                SendPacket(new KeepAlivePacket
                {
                    Timestamp = timestamp
                });

                LastKeepAlive = timestamp;
                LastKeepAliveSendAt = DateTime.UtcNow;
            }
        }
    }
}