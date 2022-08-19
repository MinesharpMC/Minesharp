using Minesharp.Events;

namespace Minesharp.Plugins;

public interface IPluginBuilder
{
    void AddListener<TEvent, TListener>() where TListener : class, IEventListener<TEvent> where TEvent : IEvent;
}