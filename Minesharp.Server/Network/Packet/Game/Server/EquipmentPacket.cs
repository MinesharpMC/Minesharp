using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EquipmentPacket : GamePacket
{
    public EquipmentPacket()
    {
    }

    public EquipmentPacket(int entityId, EquipmentSlot slot, ItemStack item)
    {
        EntityId = entityId;
        Slot = slot;
        Item = item;
    }

    public int EntityId { get; init; }
    public EquipmentSlot Slot { get; init; }
    public ItemStack Item { get; init; }
}

public class EquipmentPacketCodec : GamePacketEncoder<EquipmentPacket>
{
    public override int PacketId => 0x50;

    protected override void Encode(EquipmentPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteVarIntEnum(packet.Slot);
        buffer.WriteItemStack(packet.Item);
    }
}