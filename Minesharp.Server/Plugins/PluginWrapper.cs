namespace Minesharp.Server.Plugins;

public class PluginWrapper
{
    public string Name { get; init; }
    public IServiceProvider Services { get; init; }
}