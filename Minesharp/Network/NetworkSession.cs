using System.Collections.Concurrent;
using DotNetty.Transport.Channels;
using Minesharp.Game.Entities;
using Minesharp.Network.Processor;
using Minesharp.Packet;
using Minesharp.Packet.Common;
using Minesharp.Packet.Game.Server;

namespace Minesharp.Network;

public sealed class NetworkSession
{
    private readonly IChannel channel;
    private readonly ConcurrentQueue<IPacket> packets = new();
    private readonly PacketProcessorManager processorManager;

    public NetworkSession(IChannel channel, PacketProcessorManager processorManager)
    {
        this.channel = channel;
        this.processorManager = processorManager;
    }

    public ProtocolType Protocol { get; set; }
    public Player Player { get; set; }

    public long LastKeepAlive { get; private set; }
    public DateTime LastKeepAliveAt { get; private set; }

    public void Enqueue(IPacket packet)
    {
        packets.Enqueue(packet);
    }

    public void SendPacket(IPacket packet)
    {
        channel.WriteAndFlushAsync(packet);
    }

    public void Tick()
    {
        while (packets.TryDequeue(out var packet))
        {
            var processor = processorManager.GetProcessor(packet.GetType());
            if (processor is null)
            {
                continue;
            }

            processor.Process(this, packet);
        }

        if (LastKeepAliveAt.AddSeconds(10) < DateTime.UtcNow)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            SendPacket(new KeepAliveRequestPacket
            {
                Timestamp = timestamp
            });

            LastKeepAlive = timestamp;
            LastKeepAliveAt = DateTime.UtcNow;
        }
    }

    public void Disconnect()
    {
        channel.CloseAsync();
    }
}