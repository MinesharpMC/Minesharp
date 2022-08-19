using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class ChangeHeldItemPacket : GamePacket
{
    public short Slot { get; init; }
}

public class ChangeHeldItemPacketCodec : GamePacketCodec<ChangeHeldItemPacket>
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

    protected override void Encode(ChangeHeldItemPacket packet, IByteBuffer buffer)
    {
        buffer.WriteShort(packet.Slot);
    }
}