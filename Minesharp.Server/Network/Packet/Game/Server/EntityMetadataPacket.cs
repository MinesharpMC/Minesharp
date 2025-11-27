using DotNetty.Buffers;
using Minesharp.Server.Entities.Metadata;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class EntityMetadataPacket : GamePacket
{
    public EntityMetadataPacket(int entityId, IList<KeyValuePair<MetadataIndex, object>> entries)
    {
        EntityId = entityId;
        Entries = entries;
    }

    public int EntityId { get; init; }
    public IList<KeyValuePair<MetadataIndex, object>> Entries { get; init; }
}

public class EntityMetadataPacketCodec : GamePacketEncoder<EntityMetadataPacket>
{
    public override int PacketId => 0x4D;

    protected override void Encode(EntityMetadataPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.EntityId);
        buffer.WriteMetadata(packet.Entries);
    }
}