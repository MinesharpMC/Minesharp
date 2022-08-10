using DotNetty.Buffers;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Client.Play;

public class PluginPacket : ClientPacket
{
    public string Channel { get; init; }
    public IByteBuffer Data { get; init; }
}

public class PluginCreator : PacketCreator<PluginPacket>
{
    public override int PacketId => 0xC;
    public override NetworkProtocol Protocol => NetworkProtocol.Play;

    protected override PluginPacket CreatePacket(IByteBuffer buffer)
    {
        var channel = buffer.ReadString();
        var data = buffer.ReadBytes(buffer.ReadableBytes);

        return new PluginPacket
        {
            Channel = channel,
            Data = data
        };
    }
}