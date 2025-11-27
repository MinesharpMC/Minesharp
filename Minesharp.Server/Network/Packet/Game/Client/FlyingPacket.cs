using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class FlyingPacket : GamePacket
{
    public bool IsGrounded { get; init; }
}

public class FlyingPacketCodec : GamePacketDecoder<FlyingPacket>
{
    public override int PacketId { get; } = 0x16;
    
    protected override FlyingPacket Decode(IByteBuffer buffer)
    {
        var grounded = buffer.ReadBoolean();
        
        return new FlyingPacket
        {
            IsGrounded = grounded
        };
    }
}