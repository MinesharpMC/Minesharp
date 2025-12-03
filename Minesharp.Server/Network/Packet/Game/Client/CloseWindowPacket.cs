using DotNetty.Buffers;

namespace Minesharp.Server.Network.Packet.Game.Client;

public class CloseWindowPacket : GamePacket
{
    public byte WindowId { get; init; }
}

public class CloseWindowPacketCodec : GamePacketDecoder<CloseWindowPacket>
{
    public override int PacketId { get; } = 0x0B;
    
    protected override CloseWindowPacket Decode(IByteBuffer buffer)
    {
        var windowId = buffer.ReadByte();
        
        return new CloseWindowPacket
        {
            WindowId = windowId
        };
    }
}