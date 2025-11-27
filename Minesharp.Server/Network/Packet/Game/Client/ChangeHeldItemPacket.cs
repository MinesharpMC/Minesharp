using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class ChangeHeldItemPacket : GamePacket
{
    public short Slot { get; init; }
}

public class ChangeHeldItemPacketCodec : GamePacketDecoder<ChangeHeldItemPacket>
{
    public override int PacketId => 0x27;

    protected override ChangeHeldItemPacket Decode(IByteBuffer buffer)
    {
        var slot = buffer.ReadShort();

        return new ChangeHeldItemPacket
        {
            Slot = slot
        };
    }
}