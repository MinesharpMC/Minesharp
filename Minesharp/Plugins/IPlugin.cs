using Microsoft.Extensions.DependencyInjection;

namespace Minesharp.Plugins;

/// <summary>
/// Interface implemented by all plugins
/// </summary>
public interface IPlugin
{
    void ConfigureDependencies(IServiceCollection services);
    void Configure(IPluginBuilder plugin);
}