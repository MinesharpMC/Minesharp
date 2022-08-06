using System.Reflection;
using DotNetty.Buffers;
using Minesharp.Extension;
using Minesharp.Network.Common;
using Minesharp.Network.Packet.Client;
using Minesharp.Network.Packet.Server;

namespace Minesharp.Network.Packet;

public class PacketFactory
{
    private readonly Dictionary<NetworkProtocol, Dictionary<int, PacketCreator>> packetCreators;
    private readonly Dictionary<NetworkProtocol, Dictionary<Type, BufferCreator>> bufferCreators;

    public PacketFactory(IEnumerable<PacketCreator> packetCreators, IEnumerable<BufferCreator> bufferCreators)
    {
        this.packetCreators = packetCreators.ToNestedDictionary(x => x.Protocol, x => x.PacketId);
        this.bufferCreators = bufferCreators.ToNestedDictionary(x => x.Protocol, x => x.PacketType);
    }
    
    public IByteBuffer CreateBuffer(NetworkProtocol protocol, ServerPacket packet)
    {
        var type = packet.GetType();
        var creator = bufferCreators.GetValueOrDefault(protocol)?.GetValueOrDefault(type);
        if (creator is null)
        {
            return default;
        }

        var buffer = Unpooled.Buffer();
        
        buffer.WriteVarInt(creator.PacketId);
        creator.Create(packet, buffer);

        return buffer;
    }

    public ClientPacket CreatePacket(NetworkProtocol protocol, IByteBuffer buffer)
    {
        var packetId = buffer.ReadVarInt();
        var creator = packetCreators.GetValueOrDefault(protocol)?.GetValueOrDefault(packetId);
        if (creator is null)
        {
            return default;
        }

        return creator.Create(buffer);
    }
}
