using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Storages;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class InventoryClickPacket : GamePacket
{
    public byte Window { get; init; }
    public int State { get; init; }
    public short Slot { get; init; }
    public byte Button { get; init; }
    public int Mode { get; init; }
    public Dictionary<short, ItemStack> Items { get; init; }
    public ItemStack CarriedItem { get; init; }
}

public class InventoryClickPacketCodec : GamePacketDecoder<InventoryClickPacket>
{
    public override int PacketId => 0x0A;

    protected override InventoryClickPacket Decode(IByteBuffer buffer)
    {
        var window = buffer.ReadByte();
        var state = buffer.ReadVarInt();
        var slot = buffer.ReadShort();
        var button = buffer.ReadByte();
        var mode = buffer.ReadVarInt();

        var length = buffer.ReadVarInt();
        var items = new Dictionary<short, ItemStack>();
        for (var i = 0; i < length; i++)
        {
            var slotIndex = buffer.ReadShort();
            var slotItem = buffer.ReadItemStack();

            items[slotIndex] = slotItem;
        }

        var item = buffer.ReadItemStack();

        return new InventoryClickPacket
        {
            Window = window,
            State = state,
            Slot = slot,
            Button = button,
            Mode = mode,
            Items = items,
            CarriedItem = item
        };
    }
}