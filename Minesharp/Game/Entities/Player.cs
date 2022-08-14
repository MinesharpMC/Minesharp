using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Chunks;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Packet;

namespace Minesharp.Game.Entities;

public sealed class Player
{
    private readonly NetworkSession session;
    private readonly ChunkProcessor chunkProcessor;

    public Player(NetworkSession session)
    {
        this.session = session;
        this.chunkProcessor = new ChunkProcessor(this);
    }

    public int Id { get; init; }
    public Guid UniqueId { get; init; }
    public string Username { get; set; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    public World World { get; set; }
    public Server Server { get; init; }
    public GameMode GameMode { get; set; }
    public string Locale { get; set; }
    public byte ViewDistance { get; set; }
    public Hand MainHand { get; set; }

    public void SendPacket(IPacket packet)
    {
        session.SendPacket(packet);
    }

    public void Tick()
    {
        chunkProcessor.Tick();
    }
}