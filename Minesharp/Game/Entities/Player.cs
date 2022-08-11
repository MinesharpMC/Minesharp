using Minesharp.Chat.Component;
using Minesharp.Game.Worlds;
using Minesharp.Network;
using Minesharp.Network.Packet.Server.Play;

namespace Minesharp.Game.Entities;

public sealed class Player
{
    private readonly Server server;
    private readonly NetworkSession session;

    public Player(Server server, NetworkSession session)
    {
        this.server = server;
        this.session = session;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public World World { get; set; }
    public Position Position { get; set; }
    public Rotation Rotation { get; set; }
    
    public DateTime LastKeepAliveSendAt { get; private set; }
    public long LastKeepAlive { get; private set; }
    
    public void Tick()
    {
        session.Tick();
        
        if (LastKeepAliveSendAt.AddSeconds(10) < DateTime.UtcNow)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            session.SendPacket(new KeepAlivePacket
            {
                Timestamp = timestamp
            });

            LastKeepAlive = timestamp;
            LastKeepAliveSendAt = DateTime.UtcNow;
        }
    }

    public void SendMessage(string message)
    {
        session.SendPacket(new SystemMessagePacket
        {
            Chat = new TextComponent
            {
                Text = message
            },
        });
    }
}