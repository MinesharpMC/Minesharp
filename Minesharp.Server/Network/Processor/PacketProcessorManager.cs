namespace Minesharp.Server.Network.Processor;

public class PacketProcessorManager
{
    private readonly Dictionary<Type, IPacketProcessor> processors;

    public PacketProcessorManager(IEnumerable<IPacketProcessor> processors)
    {
        this.processors = processors.ToDictionary(x => x.PacketType);
    }

    public IPacketProcessor GetProcessor(Type packetType)
    {
        return processors.GetValueOrDefault(packetType);
    }
}