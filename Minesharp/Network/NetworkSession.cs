using System.Collections.Concurrent;
using DotNetty.Transport.Channels;
using Minesharp.Common.Enum;
using Minesharp.Game.Entities;
using Minesharp.Network.Processor;
using Minesharp.Packet;
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

    public Guid Id { get; } = Guid.NewGuid();
    public Protocol Protocol { get; set; }
    public Player Player { get; set; }

    public long LastKeepAlive { get; set; }
    public DateTime LastKeepAliveSendAt { get; set; }
    public DateTime LastKeepAliveReceivedAt { get; set; } = DateTime.UtcNow;

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

        if (LastKeepAliveSendAt.AddSeconds(10) < DateTime.UtcNow)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            SendPacket(new KeepAliveRequestPacket
            {
                Timestamp = timestamp
            });

            LastKeepAlive = timestamp;
            LastKeepAliveSendAt = DateTime.UtcNow;
        }

        if (LastKeepAliveReceivedAt.AddSeconds(30) < DateTime.UtcNow)
        {
            Disconnect();
        }
    }

    public void Disconnect()
    {
        channel.CloseAsync();
    }
}