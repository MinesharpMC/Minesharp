namespace Minesharp.Events;

public interface IEventListener<in T> where T : IEvent
{
    void Handle(T e);
}