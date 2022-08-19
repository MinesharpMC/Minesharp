using Minesharp.Plugins;

namespace Minesharp.Server.Game.Plugins;

public class PluginWrapper
{
    public string Name { get; init; }
    public IPlugin Plugin { get; init; }
    public IServiceProvider Services { get; init; }
}