using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class RemoveEntitiesPacket : GamePacket
{
    public IList<int> Entities { get; init; }

    public RemoveEntitiesPacket()
    {
    }

    public RemoveEntitiesPacket(IList<int> entities)
    {
        Entities = entities;
    }
}

public class RemoveEntitiesPacketCodec : GamePacketCodec<RemoveEntitiesPacket>
{
    public override int PacketId => 0x38;

    protected override RemoveEntitiesPacket Decode(IByteBuffer buffer)
    {
        var entities = buffer.ReadVarIntArray();

        return new RemoveEntitiesPacket
        {
            Entities = entities
        };
    }

    protected override void Encode(RemoveEntitiesPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarIntArray(packet.Entities);
    }
}