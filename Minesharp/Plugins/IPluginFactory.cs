using Microsoft.Extensions.DependencyInjection;

namespace Minesharp.Plugins;

public interface IPluginFactory
{
    void Configure(IServiceCollection services);
}