using DotNetty.Buffers;
using Minesharp.Chat.Component;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Login.Server;

public sealed class DisconnectPacket : LoginPacket
{
    /// <summary>
    ///     The reason why the player was disconnected.
    /// </summary>
    public TextComponent Reason { get; init; }
}

public sealed class DisconnectPacketCodec : LoginPacketCodec<DisconnectPacket>
{
    public override int PacketId => 0x0;

    protected override DisconnectPacket Decode(IByteBuffer buffer)
    {
        var reason = buffer.ReadComponent<TextComponent>();

        return new DisconnectPacket
        {
            Reason = reason
        };
    }

    protected override void Encode(DisconnectPacket packet, IByteBuffer buffer)
    {
        buffer.WriteComponent(packet.Reason);
    }
}