using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class HealthPacket : GamePacket
{
    public float Health { get; init; }
    public int Food { get; init; }
    public float Saturation { get; init; }

    public HealthPacket()
    {
    }

    public HealthPacket(float health, int food, float saturation)
    {
        Health = health;
        Food = food;
        Saturation = saturation;
    }
}

public sealed class HealthPacketCodec : GamePacketCodec<HealthPacket>
{
    public override int PacketId => 0x52;

    protected override HealthPacket Decode(IByteBuffer buffer)
    {
        var health = buffer.ReadFloat();
        var food = buffer.ReadVarInt();
        var saturation = buffer.ReadFloat();

        return new HealthPacket
        {
            Health = health,
            Food = food,
            Saturation = saturation
        };
    }

    protected override void Encode(HealthPacket packet, IByteBuffer buffer)
    {
        buffer.WriteFloat(packet.Health);
        buffer.WriteVarInt(packet.Food);
        buffer.WriteFloat(packet.Saturation);
    }
}