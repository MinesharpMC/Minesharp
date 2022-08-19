using Minesharp.Events;
using Minesharp.Plugins;

namespace Minesharp.Server.Plugins;

public class PluginBuilder : IPluginBuilder
{
    private readonly IServiceCollection services;

    public PluginBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public void AddListener<TEvent, TListener>() where TEvent : IEvent where TListener : class, IEventListener<TEvent>
    {
        services.AddTransient<IEventListener<TEvent>, TListener>();
    }
}