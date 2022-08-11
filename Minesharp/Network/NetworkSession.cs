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
    public NetworkProtocol Protocol { get; set; }
    public Player Player { get; set; }

    private readonly IChannel channel;
    private readonly ConcurrentQueue<ClientPacket> packetQueue = new();
    private readonly PacketProcessorManager processorManager;

    public NetworkSession(IChannel channel, PacketProcessorManager processorManager)
    {
        this.channel = channel;
        this.processorManager = processorManager;
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

    public void ReceivePacket(ClientPacket packet)
    {
        packetQueue.Enqueue(packet);
    }

    public void Tick()
    {
        while (packetQueue.TryDequeue(out var packet))
        {
            var processor = processorManager.GetProcessorForPacket(packet);
            if (processor is null)
            {
                continue;
            }
            
            processor.Process(this, packet);
        }
    }
}