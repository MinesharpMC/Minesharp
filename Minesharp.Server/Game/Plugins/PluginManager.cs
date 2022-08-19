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
            var dependencyType = types.FirstOrDefault(x => typeof(IPluginFactory).IsAssignableFrom(x));

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
                var dependency = Activator.CreateInstance(dependencyType) as IPluginFactory;
                if (dependency is not null)
                {
                    dependency.Configure(services);
                }
            }

            var provider = services.BuildServiceProvider();
            
            var plugin = provider.GetService<IPlugin>();
            if (plugin is null)
            {
                Log.Error("Failed to get plugin instance");
                continue;
            }
            
            plugin.Start();
            plugins.Add(new PluginWrapper
            {
                Plugin = plugin,
                Services = provider,
                Name = configuration.Name
            });

            Log.Information("Successfully started plugin {name} ({assembly})", configuration.Name, assembly.GetName().Name);
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

    public void StopAll()
    {
        foreach (var wrapper in plugins)
        {
            wrapper.Plugin.Stop();
        }
    }
}