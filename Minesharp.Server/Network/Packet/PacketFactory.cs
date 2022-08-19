using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet;

public sealed class PacketFactory
{
    private readonly Dictionary<Protocol, Dictionary<int, IPacketCodec>> codecsById;
    private readonly Dictionary<Protocol, Dictionary<Type, IPacketCodec>> codecsByType;

    public PacketFactory(IEnumerable<IPacketCodec> codecs)
    {
        codecsById = codecs.ToNestedDictionary(x => x.Protocol, x => x.PacketId);
        codecsByType = codecs.ToNestedDictionary(x => x.Protocol, x => x.PacketType);
    }

    public IPacket Decode(Protocol protocol, IByteBuffer buffer)
    {
        var packetId = buffer.ReadVarInt();
        var codec = codecsById.GetValueOrDefault(protocol)?.GetValueOrDefault(packetId);
        if (codec is null)
        {
            return default;
        }

        return codec.Decode(buffer);
    }

    public void Encode(Protocol protocol, IByteBuffer buffer, IPacket packet)
    {
        var type = packet.GetType();
        var codec = codecsByType.GetValueOrDefault(protocol)?.GetValueOrDefault(type);
        if (codec is null)
        {
            return;
        }

        buffer.WriteVarInt(codec.PacketId);
        codec.Encode(packet, buffer);
    }
}