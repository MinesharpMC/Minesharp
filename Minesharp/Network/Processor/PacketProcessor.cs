using Minesharp.Network.Packet.Client;

namespace Minesharp.Network.Processor;

public abstract class PacketProcessor
{
    public abstract Type PacketType { get; }
    public abstract void Process(NetworkClient client, ClientPacket packet);
}

public abstract class PacketProcessor<T> : PacketProcessor where T : ClientPacket
{
    public override Type PacketType { get; } = typeof(T);
    
    public override void Process(NetworkClient client, ClientPacket packet)
    {
        Process(client, (T)packet);
    }

    protected abstract void Process(NetworkClient client, T packet);
}