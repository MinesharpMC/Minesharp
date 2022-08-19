using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Storages;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EquipmentPacket : GamePacket
{
    public EquipmentPacket()
    {
    }

    public EquipmentPacket(int entityId, EquipmentSlot slot, Stack item)
    {
        EntityId = entityId;
        Slot = slot;
        Item = item;
    }

    public int EntityId { get; init; }
    public EquipmentSlot Slot { get; init; }
    public Stack Item { get; init; }
}

public class EquipmentPacketCodec : GamePacketCodec<EquipmentPacket>
{
    public override int PacketId => 0x50;

    protected override EquipmentPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(EquipmentPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteVarIntEnum(packet.Slot);
        buffer.WriteStack(packet.Item);
    }
}