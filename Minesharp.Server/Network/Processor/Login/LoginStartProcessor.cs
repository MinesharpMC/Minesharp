using Minesharp.Chat;
using Minesharp.Events.Player;
using Minesharp.Server.Extension;
using Minesharp.Server.Game;
using Minesharp.Server.Game.Entities;
using Minesharp.Server.Game.Storages;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Network.Packet.Login.Client;
using Minesharp.Server.Network.Packet.Login.Server;
using Minesharp.Server.Utility;

namespace Minesharp.Server.Network.Processor.Login;

public class LoginStartProcessor : PacketProcessor<LoginStartPacket>
{
    private readonly GameServer server;

    public LoginStartProcessor(GameServer server)
    {
        this.server = server;
    }

    protected override void Process(NetworkSession session, LoginStartPacket packet)
    {
        var world = server.GetDefaultWorld();
        var player = session.Player = new Player(session)
        {
            Id = server.GetNextEntityId(),
            UniqueId = Guid.NewGuid(),
            Username = packet.Username,
            Position = world.SpawnPosition,
            Rotation = world.SpawnRotation,
            Server = server,
            GameMode = world.GameMode,
            World = world,
            ViewDistance = server.ViewDistance,
            Inventory = new PlayerStorage
            {
                [36] = new(Material.Stone),
                [37] = new(Material.GrassBlock),
                [38] = new(Material.Dirt),
                [39] = new(Material.Bedrock),
                MainHandSlot = 36
            }
        };

        session.SendPacket(new LoginSuccessPacket
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

        var e = server.CallEvent(new PlayerJoinEvent
        {
            Player = player,
            Message = $"{player.Username} joined the game"
        });

        if (e.IsCancelled)
        {
            session.Disconnect();
            return;
        }

        world.AddPlayer(player);
        server.AddPlayer(player);

        server.BroadcastMessage(e.Message, ChatColor.Yellow);
        server.BroadcastPlayerListAdd(player);

        player.RefreshChunks();

        player.SendPlayerList();
        player.SendInventory();
        player.SendPosition();
    }
}