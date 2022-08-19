using Microsoft.Extensions.DependencyInjection;
using Minesharp.Events;

namespace Minesharp.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddListener<TEvent, TEventListener>(this IServiceCollection services) where TEventListener : class, IEventListener<TEvent> where TEvent : IEvent
    {
        services.AddTransient<IEventListener<TEvent>, TEventListener>();
    }
}