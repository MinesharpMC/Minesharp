using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public class SpawnPlayerPacket : GamePacket
{
    public SpawnPlayerPacket()
    {
    }

    public SpawnPlayerPacket(int id, Guid uniqueId, Position position, Rotation rotation)
    {
        Id = id;
        UniqueId = uniqueId;
        Position = position;
        Rotation = rotation;
    }

    public int Id { get; init; }
    public Guid UniqueId { get; init; }
    public Position Position { get; init; }
    public Rotation Rotation { get; init; }
}

public class SpawnPlayerPacketCodec : GamePacketCodec<SpawnPlayerPacket>
{
    public override int PacketId => 0x02;

    protected override SpawnPlayerPacket Decode(IByteBuffer buffer)
    {
        var id = buffer.ReadVarInt();
        var uniqueId = buffer.ReadGuid();
        var position = buffer.ReadPosition();

        var yaw = buffer.ReadByte();
        var pitch = buffer.ReadByte();

        return new SpawnPlayerPacket
        {
            Id = id,
            UniqueId = uniqueId,
            Position = position,
            Rotation = new Rotation(pitch, yaw)
        };
    }

    protected override void Encode(SpawnPlayerPacket packet, IByteBuffer buffer)
    {
        buffer.WriteVarInt(packet.Id);
        buffer.WriteGuid(packet.UniqueId);
        buffer.WritePosition(packet.Position);
        buffer.WriteAngle(packet.Rotation);
    }
}