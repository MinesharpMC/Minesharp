using DotNetty.Buffers;
using Minesharp.Server.Extension;

namespace Minesharp.Server.Network.Packet.Game.Server;

public sealed class HealthPacket : GamePacket
{
    public HealthPacket()
    {
    }

    public HealthPacket(float health, int food, float saturation)
    {
        Health = health;
        Food = food;
        Saturation = saturation;
    }

    public float Health { get; init; }
    public int Food { get; init; }
    public float Saturation { get; init; }
}

public sealed class HealthPacketCodec : GamePacketEncoder<HealthPacket>
{
    public override int PacketId => 0x52;

    protected override void Encode(HealthPacket packet, IByteBuffer buffer)
    {
        buffer.WriteFloat(packet.Health);
        buffer.WriteVarInt(packet.Food);
        buffer.WriteFloat(packet.Saturation);
    }
}