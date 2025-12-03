using Minesharp.Chat;
using Minesharp.Events.Player;
using Minesharp.Server.Common;
using Minesharp.Server.Entities;
using Minesharp.Server.Extension;
using Minesharp.Server.Network.Packet.Game.Server;
using Minesharp.Server.Network.Packet.Login.Client;
using Minesharp.Server.Network.Packet.Login.Server;
using Minesharp.Server.Storages;
using Minesharp.Storages;

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
        var position = world.SpawnPosition;
        
        var player = session.Player = new Player(session, world, position)
        {
            Username = packet.Username,
            Rotation = world.SpawnRotation,
            GameMode = world.GameMode,
            ViewDistance = server.ViewDistance,
        };
        
        player.Inventory.AddItem(new ItemStack(Material.StonePickaxe));

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

        var e = server.Publish(new PlayerJoinEvent(player));
        if (e.IsCancelled)
        {
            session.Disconnect();
            return;
        }

        world.AddEntity(player);

        server.BroadcastMessage(e.Message ?? $"{player.Username} joined the game", ChatColor.Yellow);
        server.BroadcastPlayerListAdd(player);

        player.RefreshChunks();

        player.SendPlayerList();
        player.UpdateInventory();
        player.SendPosition();
    }
}