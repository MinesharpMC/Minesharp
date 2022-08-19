using Minesharp.Server.Network.Packet;

namespace Minesharp.Server.Network.Processor;

public interface IPacketProcessor
{
    Type PacketType { get; }
    void Process(NetworkSession session, IPacket packet);
}

public abstract class PacketProcessor<T> : IPacketProcessor where T : IPacket
{
    public Type PacketType { get; } = typeof(T);

    public void Process(NetworkSession session, IPacket packet)
    {
        Process(session, (T)packet);
    }

    protected abstract void Process(NetworkSession session, T packet);
}