using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Storages;
using Minesharp.Storages;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateInventorySlotPacket : GamePacket
{
    public byte Window { get; init; }
    public int State { get; init; }
    public short Slot { get; init; }
    public ItemStack Item { get; init; }
}

public class UpdateInventorySlotPacketCodec : GamePacketEncoder<UpdateInventorySlotPacket>
{
    public override int PacketId => 0x13;

    protected override void Encode(UpdateInventorySlotPacket packet, IByteBuffer buffer)
    {
        buffer.WriteByte(packet.Window);
        buffer.WriteVarInt(packet.State);
        buffer.WriteShort(packet.Slot);
        buffer.WriteItemStack(packet.Item);
    }
}