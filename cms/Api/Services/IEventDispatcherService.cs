namespace Cms.Api.Services;


public interface IEventDispatcherService
{
  void Dispatch<TEvent>(TEvent eventToDispatch)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new();

  void Dispatch<TEvent>(TEvent eventToDispatch, uint delayInSeconds)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new();

  void Dispatch<TEvent>(TEvent eventToDispatch, Func<TEvent, Task>? eventHandler)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new();


  Task InvokeEventHandlerAsync<TEvent>(TEvent eventToProcess, Func<TEvent, Task>? eventHandler = null)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new();
}