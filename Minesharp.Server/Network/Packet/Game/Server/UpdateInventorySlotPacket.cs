using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Storages;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateInventorySlotPacket : GamePacket
{
    public byte Window { get; init; }
    public int State { get; init; }
    public short Slot { get; init; }
    public Stack Item { get; init; }
}

public class UpdateInventorySlotPacketCodec : GamePacketCodec<UpdateInventorySlotPacket>
{
    public override int PacketId => 0x13;

    protected override UpdateInventorySlotPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(UpdateInventorySlotPacket packet, IByteBuffer buffer)
    {
        buffer.WriteByte(packet.Window);
        buffer.WriteVarInt(packet.State);
        buffer.WriteShort(packet.Slot);
        buffer.WriteStack(packet.Item);
    }
}