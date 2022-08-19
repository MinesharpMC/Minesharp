using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Minesharp.Plugins;
using Serilog;
using Serilog.Filters;
using YamlDotNet.Serialization;

namespace Minesharp.Server.Game.Plugins;

public class PluginManager
{
    private readonly IDeserializer deserializer = new Deserializer();
    private readonly List<IPlugin> plugins = new();
    private readonly string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
    
    private readonly GameServer server;

    public PluginManager(GameServer server)
    {
        this.server = server;
    }

    public void StartAll()
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
            
            var assembly = Assembly.LoadFile(Path.Combine(directory, configuration.Assembly));
            var types = assembly.GetTypes();

            var pluginType = types.FirstOrDefault(x => typeof(IPlugin).IsAssignableFrom(x));
            var dependencyType = types.FirstOrDefault(x => typeof(IPluginDependency).IsAssignableFrom(x));

            if (pluginType == null)
            {
                Log.Error("Failed to load {plugin}", configuration.Name);
                continue;
            }
            
            var services = new ServiceCollection();

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
            services.TryAddSingleton(typeof(IPlugin), pluginType);
            
            if (dependencyType is not null)
            {
                var dependency = Activator.CreateInstance(dependencyType) as IPluginDependency;
                if (dependency is not null)
                {
                    dependency.ConfigureServices(services);
                }
            }

            var plugin = services.BuildServiceProvider().GetService<IPlugin>();
            if (plugin is null)
            {
                Log.Error("Failed to get plugin instance");
                continue;
            }
            
            plugin.Start();
            plugins.Add(plugin);

            Log.Information("Successfully started plugin {name} ({assembly})", configuration.Name, assembly.GetName().Name);
        }
    }

    public void StopAll()
    {
        foreach (var plugin in plugins)
        {
            plugin.Stop();
        }
    }
}