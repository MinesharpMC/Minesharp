using Minesharp.Chat;
using Minesharp.Chat.Component;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Extension;
using Minesharp.Game;
using Minesharp.Game.Entities;
using Minesharp.Packet.Game.Server;
using Minesharp.Packet.Login.Client;
using Minesharp.Packet.Login.Server;
using Minesharp.Utility;

namespace Minesharp.Network.Processor.Login;

public class LoginStartProcessor : PacketProcessor<LoginStartPacket>
{
    private readonly Server server;

    public LoginStartProcessor(Server server)
    {
        this.server = server;
    }

    protected override void Process(NetworkSession session, LoginStartPacket packet)
    {
        var world = server.GetDefaultWorld();
        var player = session.Player = new Player(session)
        {
            Id = Server.GetNextEntityId(),
            UniqueId = Guid.NewGuid(),
            Username = packet.Username,
            Position = new Position(0, -55, 0),
            Rotation = new Rotation(0, 0),
            Server = server,
            GameMode = world.GameMode,
            World = world,
            ViewDistance = server.ViewDistance
        };

        player.SendPacket(new LoginSuccessPacket
        {
            Id = player.UniqueId,
            Username = packet.Username
        });

        session.Protocol = Protocol.Game;

        player.SendPacket(new JoinGamePacket
        {
            Id = player.Id,
            IsHardcore = false,
            GameMode = player.GameMode,
            PreviousGameMode = GameMode.None,
            Dimensions = new[]
            {
                "minecraft:overworld"
            },
            Registry = TagUtility.CreateRegistryTag(),
            DimensionName = "minecraft:overworld",
            DimensionType = "minecraft:overworld",
            SeedHash = new byte[8],
            MaxPlayers = 1000,
            ViewDistance = player.ViewDistance,
            SimulationDistance = player.ViewDistance,
            ReducedDebug = false,
            EnabledRespawnScreen = true,
            IsDebug = false,
            IsFlat = true,
            HasDeathLocation = false
        });
        
        world.AddPlayer(player);
        server.AddPlayer(player);

        server.BroadcastMessage($"{player.Username} joined the game", ChatColor.Yellow);
        server.BroadcastPlayerListAdd(player);
        
        player.SendPlayerList();
        player.SendPosition();
    }
}