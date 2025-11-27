using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class RemoveEntitiesPacket : GamePacket
{
    public RemoveEntitiesPacket()
    {
    }

    public RemoveEntitiesPacket(IList<int> entities)
    {
        Entities = entities;
    }

    public IList<int> Entities { get; init; }
}

public class RemoveEntitiesPacketCodec : GamePacketEncoder<RemoveEntitiesPacket>
{
    public override int PacketId => 0x38;

    protected override void Encode(RemoveEntitiesPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarIntArray(packet.Entities);
    }
}