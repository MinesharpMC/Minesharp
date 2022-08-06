using Minesharp.Network.Packet.Client;

namespace Minesharp.Network.Processor;

public class PacketProcessorManager
{
    private readonly Dictionary<Type, PacketProcessor> processors;

    public PacketProcessorManager(IEnumerable<PacketProcessor> processors)
    {
        this.processors = processors.ToDictionary(x => x.PacketType);
    }

    public PacketProcessor GetProcessorForPacket(ClientPacket packet)
    {
        return processors.GetValueOrDefault(packet.GetType());
    }
}