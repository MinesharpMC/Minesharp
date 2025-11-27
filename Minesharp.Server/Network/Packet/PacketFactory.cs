using System.Diagnostics;
using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Serilog;

namespace Minesharp.Server.Network.Packet;

public sealed class PacketFactory
{
    private readonly Dictionary<Protocol, Dictionary<int, IPacketDecoder>> codecsById;
    private readonly Dictionary<Protocol, Dictionary<Type, IPacketEncoder>> codecsByType;

    public PacketFactory(IEnumerable<IPacketDecoder> decoders, IEnumerable<IPacketEncoder> encoders)
    {
        codecsById = decoders.ToNestedDictionary(x => x.Protocol, x => x.PacketId);
        codecsByType = encoders.ToNestedDictionary(x => x.Protocol, x => x.PacketType);
    }

    public IPacket Decode(Protocol protocol, IByteBuffer buffer)
    {
        var packetId = buffer.ReadVarInt();
        var codec = codecsById.GetValueOrDefault(protocol)?.GetValueOrDefault(packetId);
        
        if (codec is null)
        {
            if (packetId != 0x01)
                Log.Information("[DECODE] {Protocol}:{PacketId}", protocol, packetId.ToString("X"));
            
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
            Log.Warning("[ENCODE] {Protocol}:{PacketType} not found", protocol, type);
            return;
        }

        buffer.WriteVarInt(codec.PacketId);
        codec.Encode(packet, buffer);
    }
}