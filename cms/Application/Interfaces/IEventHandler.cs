namespace Cms.Application.Interfaces;


public interface IEventHandler<TEvent> where TEvent : class, IEvent, IEventHandler<TEvent>, new()
{
    Func<TEvent, Task>? EventHandler { get; set; }
}