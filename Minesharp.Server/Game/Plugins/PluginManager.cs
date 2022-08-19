using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Minesharp.Events;
using Minesharp.Plugins;
using Serilog;
using Serilog.Filters;
using YamlDotNet.Serialization;

namespace Minesharp.Server.Game.Plugins;

public class PluginManager
{
    private readonly IDeserializer deserializer = new Deserializer();
    private readonly List<PluginWrapper> plugins = new();
    private readonly string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
    
    private readonly GameServer server;

    public PluginManager(GameServer server)
    {
        this.server = server;
    }

    public async Task StartAll()
    {
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }
        
        var directories = Directory.GetDirectories(storagePath);
        foreach (var directory in directories)
        {
            var configurationText = File.ReadAllText(Path.Combine(directory, "plugin.yml"));
            var configuration = deserializer.Deserialize<PluginConfiguration>(configurationText);
            
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var name = args.Name.Split(',')[0];
                if (name == "Minesharp")
                {
                    return null;
                }
                
                var path = Path.GetDirectoryName(args.RequestingAssembly!.Location);
                if (path == directory)
                {
                    return Assembly.LoadFile(Path.Combine(directory, $"{name}.dll"));
                }
                
                return null;
            };
            
            var assembly = Assembly.LoadFile(Path.Combine(directory, configuration.Assembly));
            
            var types = assembly.GetTypes();

            var pluginType = types.FirstOrDefault(x => typeof(IPlugin).IsAssignableFrom(x));
            if (pluginType == null)
            {
                Log.Error("Failed to load {plugin}", configuration.Name);
                continue;
            }
            
            var services = new ServiceCollection();

            var plugin = Activator.CreateInstance(pluginType) as IPlugin;
            if (plugin is null)
            {
                continue;
            }
            
            plugin.ConfigureDependencies(services);
            plugin.Configure(new PluginBuilder(services));
            
            services.AddLogging(x =>
            {
                var logger = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}][{Plugin}] {Message:lj}{NewLine}{Exception}")
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .MinimumLevel.Is(configuration.MinimumLevel)
                    .Enrich.WithProperty("Plugin", configuration.Name)
                    .CreateLogger();
                
                x.AddSerilog(logger);
            });

            services.TryAddSingleton<IServer>(server);
            
            var provider = services.BuildServiceProvider();
            
            var hostedServices = provider.GetServices<IHostedService>();
            foreach (var hostedService in hostedServices)
            {
                await hostedService.StartAsync(CancellationToken.None);
            }
            
            plugins.Add(new PluginWrapper
            {
                Name = configuration.Name,
                Services = provider,
            });
        }
    }

    public void CallEvent<T>(T e) where T : IEvent
    {
        foreach (var plugin in plugins)
        {
            var listeners = plugin.Services.GetServices<IEventListener<T>>();
            foreach (var listener in listeners)
            {
                listener.Handle(e);
            }
        }
    }

    public async Task StopAll()
    {
        foreach (var wrapper in plugins)
        {
            var hostedServices = wrapper.Services.GetServices<IHostedService>();
            foreach (var hostedService in hostedServices)
            {
                await hostedService.StopAsync(CancellationToken.None);
            }
        }
    }
}