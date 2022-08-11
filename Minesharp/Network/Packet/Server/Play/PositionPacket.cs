using DotNetty.Buffers;
using Minesharp.Extension;

namespace Minesharp.Network.Packet.Server.Play;

public class PositionPacket : ServerPacket
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public float Pitch { get; init; }
    public float Yaw { get; init; }
    public int Flags { get; init; }
    public int TeleportId { get; init; }
    public bool DismountVehicle { get; init; }
}

public class PositionRotationCreator : BufferCreator<PositionPacket>
{
    public override int PacketId => 0x36;
    public override NetworkProtocol Protocol { get; } = NetworkProtocol.Play;
    protected override void CreateBuffer(PositionPacket packet, IByteBuffer buffer)
    {
        buffer.WriteDouble(packet.X);
        buffer.WriteDouble(packet.Y);
        buffer.WriteDouble(packet.Z);
        buffer.WriteFloat(packet.Yaw);
        buffer.WriteFloat(packet.Pitch);
        buffer.WriteByte(packet.Flags);
        buffer.WriteVarInt(packet.TeleportId);
        buffer.WriteBoolean(packet.DismountVehicle);
    }
}