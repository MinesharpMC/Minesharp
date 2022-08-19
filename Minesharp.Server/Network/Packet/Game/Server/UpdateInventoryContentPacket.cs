using DotNetty.Buffers;
using Minesharp.Server.Extension;
using Minesharp.Server.Game.Storages;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class UpdateInventoryContentPacket : GamePacket
{
    public byte Window { get; init; }
    public int State { get; set; }
    public Stack[] Items { get; init; }
    public Stack HeldItem { get; init; }
}

public class UpdateInventoryContentPacketCodec : GamePacketCodec<UpdateInventoryContentPacket>
{
    public override int PacketId => 0x11;

    protected override UpdateInventoryContentPacket Decode(IByteBuffer buffer)
    {
        throw new NotImplementedException();
    }

    protected override void Encode(UpdateInventoryContentPacket packet, IByteBuffer buffer)
    {
        buffer.WriteByte(packet.Window);
        buffer.WriteVarInt(packet.State);
        buffer.WriteVarInt(packet.Items.Length);

        foreach (var value in packet.Items)
        {
            buffer.WriteStack(value);
        }

        buffer.WriteStack(packet.HeldItem);
    }
}