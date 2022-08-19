using Microsoft.Extensions.DependencyInjection;

namespace Minesharp.Plugins;

public interface IPluginDependency
{
    void ConfigureServices(IServiceCollection services);
}